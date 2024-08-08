using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarkdownContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Author", "CreatedDate", "EditedDate", "MarkdownContent", "Title", "Version" },
                values: new object[,]
                {
                    { 1, "Sultan Dzjumajev", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "# Introduction to C#\r\n\r\nC# is a modern, object-oriented programming language developed by Microsoft. It is widely used for developing web applications, desktop applications, and more.\r\n\r\n## Features of C#\r\n\r\n- Strongly Typed\r\n- Object-Oriented\r\n- Interoperable\r\n\r\n### Sample Code\r\n\r\n```csharp\r\nusing System;\r\n\r\npublic class HelloWorld\r\n{\r\n    public static void Main()\r\n    {\r\n        Console.WriteLine(\"\"Hello, World!\"\");\r\n    }\r\n}", "Why coding makes you smarter", new byte[0] },
                    { 2, "Sultan Dzjumajev", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "\r\n# Dapr Introduction and Service-to-Service Invocation Part I\r\n\r\nWhen working with distributed systems, there are many factors to consider, including the environment you are operating in, ensuring the relevant configurations are up and running, and maintaining communication between the services and components. One common pitfall in developing a distributed system is not having a smart way to manage all the key-value pair configurations used for service-to-service communication, database connections, and other integrations. This challenge is compounded when you need to differentiate between various environment configurations and maintain each one of them.\r\n\r\nAnother difficulty in working with such systems arises when using different vendors' SDKs to integrate with their resources. This approach often results in a strong coupling to that specific infrastructure within your code, which might not be desirable, particularly when moving between local development environments and staging environments in the cloud.\r\n\r\nThis is where Dapr comes into play, addressing these problems and more.\r\n\r\nDapr is a runtime application designed to be a handy tool for distributed systems, helping achieve quality attributes like resilience, maintainability, and scalability. It accomplishes this by offering \"building blocks,\" each a coherent set of APIs tailored to specific feature.\r\n\r\n## Sidecar Pattern\r\nA significant feature of the Dapr runtime is its implementation of the sidecar pattern. In this pattern, every microservice has its own dedicated sidecar application running alongside it. The core principle here is that your main service contains the essential business logic and features, while cross-cutting concerns are offloaded to its sidecar.\r\n\r\nIn the context of Dapr, these cross-cutting concerns are particularly interesting because by offloading responsibilities to Dapr for communicating with different outbound sources, your microservice is relieved from integrating directly with vendor SDKs. Instead, it interacts with a consistent set of Dapr APIs, independent of the underlying infrastructure.\r\n\r\nEach Dapr sidecar exposes APIs for metadata, health checks, and building blocks. The metadata and health APIs are used for load balancing and discovering the appropriate destinations for communication.\r\n\r\n<figure>\r\n  <img src=\"https://docs.dapr.io/images/overview-sidecar-apis.png\" alt=\"The Dapr Sidecar APIs\" width=\"100%\"/>\r\n  <figcaption>The Dapr Sidecar APIs</figcaption>\r\n</figure>\r\n\r\n\r\n## Dapr Components\r\nDapr components are pluggable units that provide integration with various external systems and services. These components enable Dapr applications to interact with different resources such as state stores, message brokers, bindings, and observability tools. By leveraging Dapr components, developers can switch between different infrastructure providers without changing application code, enhancing the portability and flexibility of distributed applications.\r\n\r\nDapr components are declared in YAML configuration files. These declarations specify how Dapr should interact with external resources and define the metadata and configuration details necessary for integration. Components can be used by Dapr's building blocks to perform various operations like state management, publish/subscribe messaging, and resource binding.\r\n\r\nExample Component Declaration\r\nBelow is an example of a YAML file that configures a Redis state store component for use by the Dapr state management building block:\r\n\r\n```yaml\r\napiVersion: dapr.io/v1alpha1\r\nkind: Component\r\nmetadata:\r\n  name: statestore\r\n  namespace: default\r\nspec:\r\n  type: state.redis\r\n  version: v1\r\n  metadata:\r\n  - name: redisHost\r\n    value: \"localhost:6379\"\r\n  - name: redisPassword\r\n    value: \"\"\r\n```\r\nIn this example, the component is given a name (statestore) and specifies the type (state.redis) and version (v1). The metadata section contains key-value pairs needed to configure the Redis connection.\r\n\r\n\r\n## Dapr Building Blocks\r\n\r\nDapr building blocks are modular APIs that provide essential functionality for building distributed applications. They abstract away the complexities of working with various infrastructure components, allowing developers to focus on business logic. Dapr building blocks utilize Dapr Component declarations to provide various functionalities to your applications. A sevice depends on building block API and the building block depends on one or several Dapr Component declarations that determines it behavior. Summary of building blocks shown below:\r\n\r\n\r\n<figure>\r\n  <img src=\"https://docs.dapr.io/images/building_blocks.png\" alt=\"Dapr Building Blocks\" width=\"600\"/>\r\n  <figcaption>Dapr Building Blocks</figcaption>\r\n</figure>\r\n\r\n## Service-to-Service Invocation\r\nThe service-to-service invocation building block is a fundamental part of Dapr, enabling microservices to communicate with each other efficiently and reliably. This building block abstracts the complexities of service discovery and routing, allowing developers to focus on their business logic without worrying about the intricacies of network communication.\r\n\r\n* Service Discovery: Dapr automatically handles service discovery, allowing services to locate and communicate with each other without hard-coded addresses.\r\n\r\n* Load Balancing: The Dapr runtime includes load balancing mechanisms, ensuring requests are distributed evenly across service instances.\r\n\r\n* Protocol Agnostic: Supports both HTTP and gRPC, allowing developers to choose the communication protocol that best suits their needs.\r\n\r\n* Security: Dapr supports secure communication between services using mutual TLS, ensuring data privacy and integrity.\r\n\r\nBy using Dapr's service-to-service invocation building block, developers can build robust and scalable microservices architectures without the overhead of managing communication complexities.", "Dapr Introduction and Sevice-to-sevice invocation Part I", new byte[0] }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Subscribers");
        }
    }
}
