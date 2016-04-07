//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------
namespace DataStorageQueueSample
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Azure队列服务示例 - 队列服务为工作流和云服务松耦合间组建的沟通提供可靠消息机制。这个示例演示了如何执行一般的任务，包括
    /// 插入、查看、获取和删除队列信息及创建和删除队列。  
    /// 
    /// 注意：这个示例使用.NET 4.5异步编程模型来演示如何使用存储客户端库的异步API调用存储服务。 在实际的应用中这种方式
    /// 可以提高程序的响应速度。存储服务需要在调用时前面添加关键字await。
    /// 
    /// 参考文档: 
    /// - 什么是存储账号- https://www.azure.cn/documentation/articles/storage-create-storage-account/
    /// - 队列服务起步 - https://www.azure.cn/documentation/articles/storage-dotnet-how-to-use-queues/
    /// - 队列服务概念 - https://msdn.microsoft.com/zh-cn/library/dd179353.aspx
    /// - 队列服务REST API - https://msdn.microsoft.com/zh-cn/library/dd179363.aspx
    /// - 队列服务 C# API - https://msdn.microsoft.com/zh-cn/library/azure/mt347887.aspx
    /// - 存储模拟器 - https://www.azure.cn/documentation/articles/storage-use-emulator/
    /// - 使用 Async 和 Await异步编程  - http://msdn.microsoft.com/zh-cn/library/hh191443.aspx
    /// </summary>
    public class Program
    {
        // *************************************************************************************************************************
        // 使用说明: 这个示例可以在Azure存储模拟器（存储模拟器是Azure SDK安装的一部分）上运行，或者通过修改App.Config文档中的
        // AccountName（存储账号）和Key（存储密钥）的方式来使用。 
        // 
        // 使用Azure存储模拟器来运行这个示例  (默认选项)
        // 1. 点击开始按钮或者是键盘的Windows键，然后输入“Azure Storage Emulator”来寻找Azure存储模拟器，之后点击运行。       
        // 2. 设置断点，然后使用F10按钮运行这个示例. 
        // 
        // 使用Azure存储服务来运行这个示例
        // 1. 打开app.config文件, 注释掉模拟器的那一部分连接字符串(UseDevelopmentStorage=True) ，然后删掉存储服务的连接字符串的注释 (AccountName=[]...)。
        // 2. 在Azure门户网站上创建存储账号，然后修改App.Config的 [AccountName]（存储账号）和 [AccountKey]（存储密钥）。
        // 3. 设置断点，使用F10运行该示例。
        // 
        // *************************************************************************************************************************
        public static void Main(string[] args)
        {
            Console.WriteLine("Azure队列服务示例\n");

            // 创建或者引用一个已存在的队列 
            CloudQueue queue = CreateQueueAsync().Result;

            // 演示基本的队列操作方法
            BasicQueueOperationsAsync(queue).Wait();

            // 演示如何如何更新一个已排入队列的消息 
            UpdateEnqueuedMessageAsync(queue).Wait();

            // 演示高级的方法，例如处理批量的消息
            ProcessBatchOfMessagesAsync(queue).Wait();

            // 当删除一个队列时，重新创建相同名称的队列需要等待几秒钟的时间 - 因此为了快速演练示例队列并没有删除。如果你想删除队列，请取消下面代码的注释
            //DeleteQueueAsync(queue).Wait();

            Console.WriteLine("按任意键退出");
            Console.Read();
        }


        /// <summary>
        /// 为示例处理消息创建出一个队列
        /// </summary>
        /// <returns>一个CloudQueue对象</returns>
        private static async Task<CloudQueue> CreateQueueAsync()
        {
            // 通过连接字符串检索存储账号的信息
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // 创建一个queueClient来和队列服务交互
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            Console.WriteLine("1. 为该示例创建队列");
            CloudQueue queue = queueClient.GetQueueReference("samplequeue");
            try
            {
                await queue.CreateIfNotExistsAsync();
            }
            catch (StorageException ex)
            {
                Console.WriteLine("如果使用默认配置文件，请确保Azure模拟器已经启动。点击Windows键然后输入\"Azure Storage\"，找到Azure模拟器然后点击运行，之后请重新启动该示例.");
                Console.ReadLine();
                throw;
            }

            return queue;
        }

        /// <summary>
        /// 演示基本的队列操作，如添加一个信息、查看在队列前面的信息、信息出列
        /// </summary>
        /// <param name="queue">示例队列</param>
        private static async Task BasicQueueOperationsAsync(CloudQueue queue)
        {
            // 通过AddMessage方法插入一个消息到队列
            Console.WriteLine("2. 插入一个消息到队列");
            await queue.AddMessageAsync(new CloudQueueMessage("Hello World!"));

            // 通过PeekMessage查看在队列前面的消息(并不删除这条消息) - PeekMessages可以让你取1条以上的信息
            Console.WriteLine("3. 查看下一条信息");
            CloudQueueMessage peekedMessage = await queue.PeekMessageAsync();
            if (peekedMessage != null)
            {
                Console.WriteLine("查看的消息是: {0}", peekedMessage.AsString);
            }

            // 消息离开队列有两步
            // 1. 调用GetMessage，调用此方法后其他读取消息的代码将看不到这条消息，默认情况下周期30秒。
            // 2. 为了从队列中移除这条消息，你还需要调用DeleteMessage方法。
            // 这两步可以确保即使您的代码由于硬件或者软件问题处理消息失败，其他实列的代码能够得到同样的消息，然后重新处理。
            Console.WriteLine("4. 下一条消息离开队列");
            CloudQueueMessage message = await queue.GetMessageAsync();
            if (message != null)
            {
                Console.WriteLine("处理&删除消息内容: {0}", message.AsString);
                await queue.DeleteMessageAsync(message);
            }
        }

        /// <summary>
        /// 更新一个队列中的的消息和它的可见性时间。对于工作流方案，这可以使你更新任务的状态，以及通过延长可见时间使得在其他客户看到这条消息前为客户提供更多的时间继续处理消息 
        /// </summary>
        /// <param name="queue">示例队列</param>
        private static async Task UpdateEnqueuedMessageAsync(CloudQueue queue)
        {
            // 插入另一个测试消息到队列中
            Console.WriteLine("5. 插入另一个测试消息到队列 ");
            await queue.AddMessageAsync(new CloudQueueMessage("Hello World Again!"));

            Console.WriteLine("6. 改变队列消息的内容");
            CloudQueueMessage message = await queue.GetMessageAsync();
            message.SetMessageContent("更新内容.");
            await queue.UpdateMessageAsync(
                message,
                TimeSpan.Zero,  // 目的是立即可以看到更新
                MessageUpdateFields.Content |
                MessageUpdateFields.Visibility);
        }

        /// <summary>
        /// 演示添加消息、检查消息的数量及批量检索消息。在检索的过程中我们设置可见性超时时间为5分钟。
        /// 可见性超时时间：在使用GetMessageOperation但是没有调用DeleteMessage后其他客户看不到消息的时间      
        /// </summary>
        /// <param name="queue">示例队列</param>
        private static async Task ProcessBatchOfMessagesAsync(CloudQueue queue)
        {
            // 20条消息入队为了掩饰批量检索
            Console.WriteLine("7. 入队20条消息.");
            for (int i = 0; i < 20; i++)
            {
                await queue.AddMessageAsync(new CloudQueueMessage(string.Format("{0} - {1}", i, "Hello World")));
            }

            // FetchAttributes 方法可请求队列服务检索队列属性，包括消息计数
            Console.WriteLine("8. 获得队列的长度");
            queue.FetchAttributes();
            int? cachedMessageCount = queue.ApproximateMessageCount;
            Console.WriteLine("队列中消息的数量: {0}", cachedMessageCount);

            // 出列一批21条消息（最多32个），设置可见性为5分钟。注意我们出列21条消息，因为之前UpdateEnqueuedMessage方法留下了一条消息在队列中，因此我们也检索出它 
            Console.WriteLine("9. 出列21条消息，允许5分钟的时间给客户处理.");
            foreach (CloudQueueMessage msg in await queue.GetMessagesAsync(21, TimeSpan.FromMinutes(5), null, null))
            {
                Console.WriteLine("处理&删除消息内容: {0}", msg.AsString);

                // 在5分钟内处理所有的消息，每次处理完就删除消息
                await queue.DeleteMessageAsync(msg);
            }
        }
        
        /// <summary>
        /// 验证App.Config文件中的连接字符串，当使用者没有更新有效的值时抛出错误提示
        /// </summary>
        /// <param name="storageConnectionString">连接字符串</param>
        /// <returns>CloudStorageAccount 对象</returns>
        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("提供的存储信息无效，请确认App.Config文件中的AccountName和AccountKey有效后重新启动该示例");
                Console.ReadLine();
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("提供的存储信息无效，请确认App.Config文件中的AccountName和AccountKey有效后重新启动该示例");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        /// <summary>
        /// 删除示例创建队列
        /// </summary>
        /// <param name="queue">示例创建需删除的队列</param>
        private static async Task DeleteQueueAsync(CloudQueue queue)
        {
            Console.WriteLine("10. 删除队列");
            await queue.DeleteAsync();
        }
    }
}
