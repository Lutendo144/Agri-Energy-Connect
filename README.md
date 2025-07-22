								Argi-Energy connect
Argi-Energy connect is a platform that bridges the gap between green technology providers and the agriculture sector.It makes it easier for users to sign up, list products, handle events, work together on projects, and keep an eye on things as an administrator.

Tech stack:
visual studio 2022
ASP.NET Core MVC
C#
Azure SQL Database
session based authentication 
Bootstrap

Features:
1. User functionality: 
Users can register and sign-in.
Users can add items on market place.
Users can view items on market place.
Users can start new farming projects.
Users can Participate in other farmers' projects.
Users can browse training and education content

2. Admin functionality:
Admin can login.
Admin can view ,edit ,create and delete users
Admin can Manage marketplace products (filter by category and date)


Project Structure:
Argi-EnergyConnect/
|
|-Controllers/
| |_AccountController.cs
| |_AdminController.cs
| |_EducationalTrainingController.cs
| |_FundingOpportunities.cs
| |_GreenMarketplace.cs
| |_HomeController.cs
| |_ProjectCollaborationController.cs
| |_SustainableFarmingController.cs
|-Data/
|
| |_ApplicationDbContext.cs
|-ViewModels/
|
| |_AddUserViewModel.cs
| |_Admin.cs
| |_AppUser.cs
| |_DatabaseHelper.cs
| |_ErrorViewModel.cs
| |_LoginViewModel.cs
| |_MarketplaceItem.cs
| |_Product.cs
| |_ProjectCollaboration.cs
| |_ProjectComment.cs
| |_ProjectJoin.cs
| |_SignUpViewModel.cs
| |_ViewModels/
|   |_AdminLoginViewModel.cs
    |_ProjectCommentListViewModel.cs
    |_ProjectCommentViewModel.cs
|-Views/
| |_Accounts/
|
|   |_Login.cshtml
    |_SignUp.cshtml
| |_Admin/
|
|   |_CreateUser.cshtml
|   |_Dashboard.cshtml
|   |_EditUser.cshtml
|   |_Login.cshtml
|   |_ManageMarketPlace.cshtml
|   |_ManageUser.cshtml
| |_EducationalTraining/
|
|   |_Index.cshtml
| |_FundingOpportunities/
|
|   |_Index.cshtml
| |_GreenMarketplace/
|
|   |_AddItem.cshtml
|   |_Index.cshtml
| |_Home/
|
|   |_Privacy.cshtml
|   |_Index.cshtml
|   |_UserHome.cshtml
| |_ProjectCollaboration/
|
|   |_Comment.cshtml
|   |_Create.cshtml
|   |_Details.cshtml
|   |_Edit.cshtml
|   |_Index.cshtml
|   |_Join.cshtml
| |_Shared/
|
|   |_Layout.cshtml
| |_SustainableFarming/
|
|   |_Index.cshtml
|-wwwroot/
|     |_ css/
|     |_ js/
|     |_ images/
|
|- appsettings.json
|- Program.cs


How to run:
Clone repo
Open Visual Studio 2022
Set Argi-EnergyConnect as startup project
Run using Https


Developed by : Lutendo Netshirando 
Email : lutendo144@gmail.com
