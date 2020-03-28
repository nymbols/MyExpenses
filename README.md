# MyExpenses
Sample repo to manage Expenses. Written in [.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)

## Install

Follow these steps to try out this sample. The instructions are intended to be Operating System agnostic, unless called out. 

**Prepare your Environment**

1. Install the [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2) (2.2) for your operating system.
2. Git clone this repository or otherwise copy this sample to your machine: `git clone https://github.com/nymbols/myexpenses/`
3. Navigate to the sample on your machine at the command prompt or terminal.
4. Install any MSSQL Server, and update your server name in the DefaultConnection in appsettings.json.

**Run the application**

5. Run application: `dotnet run`
6. Build and and  run your application with the following two commands:
   - `dotnet build`
   - `cd myexpenses.api`
   - `dotnet run`
   
## Usage

**Swagger**
Navigate to https://localhost:5001/ in your browser to test out the features.
1. Add an expense
2. Retrieve all expenses

