IPP .NET SDK for QuickBooks V3 2.0.2

The recommended approach to install the SDK is through NuGet:

https://developer.intuit.com/docs/0025_quickbooksapi/0055_devkits/0150_ipp_.net_devkit_3.0/0001_installing_the_ipp_.net_devkit#Installing_With_NuGet

The IPP .NET SDK for QuickBooks V3 has set of .NET classes that make it easier to call QuickBooks APIs. Some of the features included in this SDK are as follows:

-Ability to perform single and batch processing of CRUD operations on all QuickBooks entities.

-Request and Response Handler has common interface with two implemented classes to handle both synchronous and asynchronous requests.

-Support for both XML and JSON Request and Response format.

-Ability to configure app settings in the configuration file requiring no additional code change.

-Support for Gzip and Deflate compression formats to improve performance of QuickBooks API calls.

-Retry policy constructors to help apps handle transient errors.

-Logging mechanisms for trace, request/response, and Azure logging.

-Sync APIs that assist with data synchronization between QuickBooks Desktop and Intuit's cloud.

-Query Filters that enable you to write LINQ queries to retrieve QuickBooks entities whose properties meet specified criteria.

-LINQ queries for accessing QuickBooks Reports.

-Sparse Update to update writable properties specified in a request and leave the others unchanged.

-Change data that enables you to retrieve a list of entities modified during specified time points.

Dependencies

.NET Framework 4.0
DevDefined.OAuth (= 0.1)
Newtonsoft.Json (= 5.0.1)