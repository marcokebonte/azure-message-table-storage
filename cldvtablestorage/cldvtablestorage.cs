using Azure.Data.Tables;
using cldvmessages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Threading.Tasks;
using CloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount;
using CloudTable = Microsoft.Azure.Cosmos.Table.CloudTable;
using CloudTableClient = Microsoft.Azure.Cosmos.Table.CloudTableClient;
using StorageCredentials = Microsoft.Azure.Cosmos.Table.StorageCredentials;
using TableEntity = Microsoft.Azure.Cosmos.Table.TableEntity;
using TableOperation = Microsoft.Azure.Cosmos.Table.TableOperation;
using TableResult = Microsoft.Azure.Cosmos.Table.TableResult;

namespace cldvtablestorage
{
    internal class cldvtablestorage
    {

        static string Identity = "";
        static string VacCentre = "";
        static string VacDate = "";
        static string VacSerial = "";
        static string normalMessage = "";
        static string reverseMessage = "";



        static void Main(string[] args)
        {

            cldvMessages cldvMessages = new cldvMessages();

            Console.WriteLine("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("Enter your age: ");
            string age = Console.ReadLine();
            Console.WriteLine("Please select your preferred format by typing (1) or (2): ");
            Console.WriteLine("EXAMPLE:");
            Console.WriteLine(" (1) 'Id:VaccinationCenter:VaccinationDate:VaccineSerialNumber'");
            Console.WriteLine(" (2) 'VaccineBarcode:VaccinationDate:VaccinationCenter:Id'");
            string userInput = Console.ReadLine();


            if (userInput.Equals("1"))
            {
                normalInput();
                cldvMessages.MessQueueNormal(Identity, VacCentre, VacDate, VacSerial);
            }
            else if (userInput.Equals("2"))
            {
                ReverseInput();
                cldvMessages.MessQueueReverse(Identity, VacCentre, VacDate, VacSerial);
            }




            var storageConnectionstring = "";//table storage connection string 
            var tableName = "";// table storage table name 

            CloudStorageAccount storageAccount;
            storageAccount = CloudStorageAccount.Parse(storageConnectionstring);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);

            RecipientEntity recipient = new RecipientEntity(userName, age)
            {
                ID = Identity,
                VaccinationCentre = VacCentre,
                VaccinationDate = VacDate,
                VaccinationSerialNumber = VacSerial

            };

            MergeUser(table, recipient).Wait();
            QueryUser(table, userName, age).Wait();

        }


        public static void normalInput()
        {
            Console.WriteLine("Enter your ID(Passport) number: ");
            Identity = Console.ReadLine();



            Console.WriteLine("Enter the vaccination centre: ");
            VacCentre = Console.ReadLine();


            Console.WriteLine("Enter the vaccination date: ");
            VacDate = Console.ReadLine();


            Console.WriteLine("Enter the vaccination serial number: ");
            VacSerial = Console.ReadLine();


        }





        public static void ReverseInput()
        {

            Console.WriteLine("Enter the vaccination serial number: ");
            VacSerial = Console.ReadLine();

            Console.WriteLine("Enter the vaccination date: ");
            VacDate = Console.ReadLine();

            Console.WriteLine("Enter the vaccination centre: ");
            VacCentre = Console.ReadLine();


            Console.WriteLine("Enter your ID(Passport) number: ");
            Identity = Console.ReadLine();


        }







        public static async Task MergeUser(CloudTable table, RecipientEntity recipient)
        {
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge((Microsoft.Azure.Cosmos.Table.ITableEntity)recipient);

            // Execute the operation.
            TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            RecipientEntity insertedCustomer = result.Result as RecipientEntity;

            Console.WriteLine("Registered successfully!!!!");
        }




        public static async Task QueryUser(CloudTable table, string Name, string age)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<RecipientEntity>(Name, age);

            TableResult result = await table.ExecuteAsync(retrieveOperation);
            RecipientEntity recipient = result.Result as RecipientEntity;

            if (recipient != null)
            {
                Console.WriteLine("Fetched \t{0}\t{1}\t{2}\t{3}",
                    recipient.PartitionKey, recipient.RowKey, recipient.VaccinationCentre, recipient.VaccinationDate, recipient.VaccinationSerialNumber);
            }
        }



        public class RecipientEntity : TableEntity
        {
            public RecipientEntity() { }
            public RecipientEntity(string Name, string Age)
            {
                PartitionKey = Name;
                RowKey = Age;
            }

            public string ID { get; set; }
            public string VaccinationCentre { get; set; }

            public string VaccinationDate { get; set; }

            public string VaccinationSerialNumber { get; set; }


        }



    }
}

