﻿using Meeter.Cli;
using Meeter.Cli.Menu;
using Meeter.Services.Notifications;

using var notificationSender = new NotificationSenderFactory()
    .Create();

notificationSender.Start();

var app = new App();
app.Setup();
app.Run();
