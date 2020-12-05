using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Phidelis.Integracao.GoogleMeet
{
    public class GoogleMeet : IDisposable
    {
        private CalendarService CalendarService { get; set; }
        private string CalendarId { get; set; } = "primary";
        private string Email { get; set; }

        public GoogleMeet(string pathJson, string email, string apiKey)
        {
            this.Email = email;
            DadosDaConta dadosDaConta;

            using (StreamReader file = File.OpenText(pathJson))
            {
                JsonSerializer serializer = new JsonSerializer();
                dadosDaConta = (DadosDaConta)serializer.Deserialize(file, typeof(DadosDaConta));
            }

            //using (StreamReader sr = new StreamReader(pathJson))
            //{
            //    // Read the stream to a string, and write the string to the console.
            //    //String line = sr.ReadToEnd();
            //    JsonSerializer serializer = new JsonSerializer();
            //    dadosDaConta = (DadosDaConta)serializer.Deserialize(sr, typeof(DadosDaConta));
            //}


            this.CalendarService = GetCalendarService(pathJson, email, dadosDaConta.project_id, apiKey);
        }

        private CalendarService GetCalendarService(string path, string email,string aplicativo, string apikey)
        {
            GoogleCredential credential = GoogleCredential.FromFile(path)
                .CreateScoped("https://www.googleapis.com/auth/calendar", "https://www.googleapis.com/auth/calendar.events")
                .CreateWithUser(email);
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = aplicativo,//calendar-api-277713
                ApiKey = apikey,//https://developers.google.com/places/web-service/get-api-key
            });

            return service;
        }

        public Event Create(string descricao, DateTime inicio, DateTime fim)
        {
            var ev = new Event();
            EventDateTime start = new EventDateTime();
            start.DateTime = inicio;
            EventDateTime end = new EventDateTime();
            end.DateTime = fim;
            ev.Start = start;
            ev.End = end;
            ev.Summary = descricao;
            ev.Description = descricao;
            ev.ConferenceData = new ConferenceData()
            {
                CreateRequest = new CreateConferenceRequest()
                {
                    RequestId = "xxx-yyyy-xxx"
                },
                ConferenceId = "xxx-yyyy-xxx",
                EntryPoints = new List<EntryPoint>() {
                    new EntryPoint()
                    {
                        Label = "meet.google.com/xxx-yyyy-xxx",
                        Uri = "https://meet.google.com/xxx-yyyy-xxx",
                        EntryPointType = "video"
                    }
                },
                ConferenceSolution = new ConferenceSolution()
                {
                    Key = new ConferenceSolutionKey()
                    {
                        Type = "hangoutsMeet",
                    },
                    Name = "Google Meet",
                    IconUri = "https://lh5.googleusercontent.com/proxy/bWvYBOb7O03a7HK5iKNEAPoUNPEXH1CHZjuOkiqxHx8OtyVn9sZ6Ktl8hfqBNQUUbCDg6T2unnsHx7RSkCyhrKgHcdoosAW8POQJm_ZEvZU9ZfAE7mZIBGr_tDlF8Z_rSzXcjTffVXg3M46v"
                },
            };
            ev.Attendees = new List<EventAttendee>()
            {
                new EventAttendee()
                {
                    Email = this.Email
                }
            };
            var exec = this.CalendarService.Events.Insert(ev, this.CalendarId);
            exec.ConferenceDataVersion = 1;
            return exec.Execute();
        }

        public void Dispose()
        {
            this.CalendarService = null;
            this.Email = null;
        }
    }
}
