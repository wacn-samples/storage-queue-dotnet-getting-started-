---
services: storage
platforms: dotnet
author: jasonnewyork

---

# .NET中使用Azure队列服务起步

队列服务为工作流和云服务松耦合间组建的沟通提供可靠消息机制。这个示例演示了如何执行一般的任务，包括插入、查看、获取和删除队列信息及创建和删除队列。 

注意：这个示例使用.NET 4.5异步编程模型来演示如何使用存储客户端库的异步API调用存储服务。 在实际的应用中这种方式可以提高程序的响应速度。存储服务需要在调用时前面添加关键字await。如果您还没有Azure订阅，请点击[此处](/pricing/1rmb-trial)申请试用的订阅账号。


## 运行这个示例

这个示例可以在Azure存储模拟器（存储模拟器是Azure SDK安装的一部分）上运行，或者通过修改App.Config文档中的AccountName（存储账号）和Key（存储密钥）的方式来使用。 
   
使用Azure存储模拟器运行该示例（默认方式）

1. 下载并安装Azure存储模拟器，[下载地址](/downloads) 
2. 点击开始按钮或者Windows键，然后输入"Azure Storage Emulator"找到存储模拟器，点击运行。
3. 设置断点，使用F10运行该示例。

使用Azure存储服务来运行这个示例

1. 打开app.config文件, 注释掉模拟器的那一部分连接字符串(UseDevelopmentStorage=True) ，然后删掉存储服务的连接字符串的注释 (AccountName=[]...)。
2. 在Azure门户网站上创建存储账号，然后修改App.Config的 [AccountName]（存储账号）和 [AccountKey]（存储密钥）。更多详细内容请阅读[这篇文章](/documentation/articles/storage-dotnet-how-to-use-blobs)
3. 设置断点，使用F10运行该示例。

## 参考文档

- [什么是存储账号](/documentation/articles/storage-create-storage-account/)
- [Blobs起步](/documentation/articles/storage-dotnet-how-to-use-blobs/)
- [队列服务概念](https://msdn.microsoft.com/zh-cn/library/dd179353.aspx) 
- [队列服务REST API](https://msdn.microsoft.com/zh-cn/library/dd179363.aspx)
- [队列服务C# API](https://msdn.microsoft.com/zh-cn/library/wa_storage_30_reference_home.aspx)
- [使用共享访问签名(SAS)委托访问](/documentation/articles/storage-dotnet-shared-access-signature-part-1/)
- [存储模拟器](/documentation/articles/storage-use-emulator/)
- [使用 Async 和 Await异步编程](http://msdn.microsoft.com/zh-cn/library/hh191443.aspx)
