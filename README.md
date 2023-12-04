# QuizWiz
### Overview
QuizWiz is a simple, cloud-native application utilizing the OpenAI Chat API. It is designed for creating and answering quizzes, leveraging .NET 8 Aspire's default template. Teachers can create quizzes on various topics, while students can search for and answer these quizzes.

### Features
* Quiz Creation by Teachers: Teachers can create quizzes on specified topics.
* Quiz Paticipation by Students: Students can search and answer quizzes by entering the teacher's email.
* Integration with OpenAI Chat API: Enhances the quiz generation with AI-driven Interactions.

### Technology Stack
* .NET8 Aspire: Provides orchestration for all applications within the solution.
* Blazor Server-Side Application: Utilizes the latest .NET 8 Blazor template.
* Azure Cosmos: Integrated with Azure CosmosDB. Generated quizzes are stored in Cosmos with the partition key of the teacher's email.
* Azure SQL Database: Uses SQL database for data storage with the latest EntityFramework Core.
* The project is leveraging clean Onion architecture where the Application layer mainly focuses on core logic. The Domain layer is for entities, all the shared models, and constants. The Infrastructure and Persistence layer is for essential services for the application, and the Presentation layer is where all the executable projects live.

### Prerequisites
* Visual Studio 2022 Preview (for .NET8)
* .NET8 SDK
* Docker

###Running the Application
1. Open the Solution: Launch Visual Studio 2022 and open the QuizWiz.sln file.
2. Build the Solution: Use Visual Studio to build the solution, ensuring all dependencies are correctly resolved.
3. Run the QuizWiz.AppHost Project: This will start the backend and frontend services.
4. Access the Application: Once the orchstration is running, access the application through the specified local port in your web browser.


