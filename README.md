# GOOGLE MEET [![Build Status](https://secure.travis-ci.org/morrisjs/morris.js.png?branch=master)](http://travis-ci.org/morrisjs/morris.js)
.NET Class Library for integration with GOOGLE MEET API <br />
.Net Framework 4.5
  
## Evento
### Criar evento
```c#
var googleMeet = new GoogleMeet("pathJson", "email_destino", "api_key");
var evento = googleMeet.Create("descrição_evento", "data_inicio", "data_fim");
```
