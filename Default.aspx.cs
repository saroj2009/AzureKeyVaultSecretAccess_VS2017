using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

using AzureResourceReport.Models;

using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;





namespace AzureKeyVaultAccess
{
    public partial class _Default : Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            // Authenticate
            string clientId = "21c9f15e-44c0-4a0c-93ce-9a1c4026c8d8";
            string clientSecret = "H8?fCvA3Rj8ZRI[?NwM?q53YP?G6nquo";
            string tenantId = "188285f7-8f1e-4c0d-a0bc-797e3e38c5b3";
            string subscriptionId = "16c4dd7c-eae8-42bc-ae66-6e5691642e32";
            string BASESECRETURI = "https://skptestkv01.vault.azure.net/secrets/"; // available from the Key Vault resource page


            AzureCredentials credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud).WithDefaultSubscription(subscriptionId);

            var keyClient = new KeyVaultClient(async (authority, resource, scope) =>
            {
                var adCredential = new ClientCredential(clientId, clientSecret);
                var authenticationContext = new AuthenticationContext(authority, null);
                return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
            });

            // Connect to Key Vault using Client ID and Secret
            KeyVaultCache keyVaultCache = new KeyVaultCache(BASESECRETURI, clientId, clientSecret);
            // Or use Managed Identity
            //AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            //KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var cacheSecret = keyVaultCache.GetCachedSecret("testsecret");

            string connectionString = cacheSecret.Result;

            string str = "Result:" + connectionString;
        }
       
    }
}