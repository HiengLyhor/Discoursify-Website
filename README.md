## Discoursify - A Discussion Platform Built with ASP.NET MVC

**Discoursify** is a dynamic web application designed to foster meaningful conversations and knowledge sharing. It empowers users to connect, share ideas, and engage in discussions on a variety of topics.

**Key Features:**

* **User Authentication:** Sign up for a free account or log in with existing credentials to participate actively.
* **Post Creation:** Craft informative and engaging posts with titles, captivating banner images, and detailed content.
* **Up/Downvoting:** Express your opinion and influence the visibility of content by upvoting valuable posts and downvoting those deemed less relevant.
* **Commenting:** Leave insightful comments to spark deeper conversations, expand upon existing ideas, and contribute to the overall discussion.
* **User Profiles:** Explore other users' profiles to gain insights, view their posts, and learn more about their activity on the platform.
* **Personalized Activity View:** Track your own actions and contributions on Discoursify, allowing you to reflect on your participation in the platform.
* **Secure Account Recovery (Optional):** Discoursify offers an optional integration with a Telegram bot named DiscoursifyBot, providing a secure One-Time Password (OTP) mechanism for account recovery if you lose your login credentials.

**Technology Stack:**

* **Backend:** C# ASP.NET MVC
* **Database:** SQL Server Management Studio (SSMS)
* **Optional:** Telegram Bot Integration

**Getting Started:**

**Prerequisites:**

* Visual Studio or your preferred C# IDE
* .NET Framework or .NET Core installed ([https://learn.microsoft.com/en-us/dotnet/core/install/windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows))
* SQL Server Management Studio (SSMS)

**Steps:**

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/HiengLyhor/Discoursify.git
   ```

2. **Configure Database Connection:**
   * Update the connection string in the appropriate file (e.g., `Web.config`) to point to your SSMS database.

3. **Configure Telegram Bot:**
   * If you choose to integrate Telegram OTP functionality, create a Telegram bot and configure it with the `Login.cs` code.

4. **Run the Application:**
   * Open the solution in Visual Studio and press `F5` to start debugging.
   * The application will typically launch in your default browser (http://localhost:<port number>).

**Get Involved!**

We're excited to see passionate individuals join the Discoursify community and contribute to its growth. Here are some ways you can get involved:

* **Report Issues:** If you encounter any bugs or have suggestions for improvement, feel free to create an issue ticket directly on this repository. Be as detailed as possible in your report to help us understand and address the issue effectively.
* **Pull Requests:** Have a brilliant idea for a new feature or a fix for an existing issue? We encourage you to submit a pull request. Make sure to follow standard Git practices and provide clear documentation for your changes.
* **Community Discussions:** Start a conversation! Share your thoughts, ideas, and feedback on the project. You can use the GitHub Discussions feature or reach out to us on relevant online forums or social media platforms.

**Contact Us:**

For further inquiries or discussions, you can reach us through the following channels:

* **Mobile Number:** +85595311461
* **Social Media:** <a>https://www.facebook.com/Hieng.Lyhorr</a>
* **Telegram Chat:** Join our Telegram chat group <a>https://t.me/DiscoursifyCommunity</a>

**Disclaimer:**

This README provides a general overview of the Discoursify project. The actual implementation details might vary depending on your specific configuration and additional functionalities you may have chosen to include.
