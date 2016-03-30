---
services: storage
platforms: dotnet
author: jasonnewyork

---

# .NET中使用Azure队列服务起步

队列服务为工作流和云服务松耦合间组建的沟通提供可靠消息机制。这个示例演示了如何执行一般的任务，包括插入、查看、获取和删除队列信息及创建和删除队列。 

注意：这个示例使用.NET 4.5异步编程模型来演示如何使用存储客户库的异步API调用存储服务。在实际的应用中这种方式可以提高程序的响应速度。调用存储服务只要添加关键字await为前缀即可。如果您还没有Azure订阅，请点击[此处](https://www.azure.cn/pricing/1rmb-trial/)申请免费的订阅账号。

## 运行这个示例

这个示例可以在Azure存储模拟器（存储模拟器是Azure SDK安装的一部分）上运行，或者通过修改App.Config文档中的存储账号和存储密匙的方式针对存储服务来使用。 
   
使用Azure存储模拟器运行该示例

1. 下载并安装Azure存储模拟器，下载地址： [https://www.azure.cn/downloads/](https://www.azure.cn/downloads/) 
2. 点击开始按钮或者Windows键，然后输入"Azure Storage Emulator"找到存储模拟机，点击运行。
3. 设置断点，使用F10运行该示例。

使用Azure存储服务来运行这个示例

1. 打来AppConfig文件然后使用第二个连接字符串。
2. 在Azure门户网站上创建存储账号，然后修改App.Config的存储账号和存储密钥。
3. 设置断点，使用F10运行该示例。

## 参考文档

- [什么是存储账号](https://www.azure.cn/documentation/articles/storage-create-storage-account/)
- [Blobs起步](http://www.azure.cn/documentation/articles/storage-dotnet-how-to-use-blobs/)
- [队列服务概念](https://msdn.microsoft.com/zh-cn/library/dd179353.aspx) 
- [队列服务REST API](https://msdn.microsoft.com/zh-cn/library/dd179363.aspx)
- [队列服务C# API](https://msdn.microsoft.com/en-us/library/wa_storage_30_reference_home.aspx)
- [使用共享访问签名(SAS)委托访问](http://www.azure.cn/documentation/articles/storage-dotnet-shared-access-signature-part-1/)
- [存储模拟器](https://www.azure.cn/documentation/articles/storage-use-emulator/)
- [使用 Async 和 Await异步编程](http://msdn.microsoft.com/zh-cn/library/hh191443.aspx)
