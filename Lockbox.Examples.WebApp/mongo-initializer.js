﻿var emailSettings = {"apiKey": "xyz", "apiUrl": "https://api.emailservice.com"}
var databaseSettings = {"connectionString": "localhost", port: 1234}
var record = {"key": "config", "value": {database: databaseSettings, email: emailSettings}}
db.Records.insert(record);