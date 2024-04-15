namespace ConnectedDrive
{
    public class ConnectedDrive
    {
        private sealed class CredentialsParameters : AccessTokenParameters
        {
            public string UserName = string.Empty;

            public string Password = string.Empty;
        }
    }
}