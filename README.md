# To test locally

Set up a `local.settings.json` file:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "EmailSender": "your@email.address",
    "SendGridApiKey": "your SendGrid API key"
  }
}