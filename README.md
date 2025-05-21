# B1CoreSL
SAP Business One Service Layer Framework for Developers

## Overview
B1CoreSL is a .NET framework designed to simplify interactions with SAP Business One Service Layer. It provides a set of methods to handle authentication, user-defined fields (UDFs), SQL queries, and general service layer requests.

## Core Functionality

### Authentication
- **LoginSAP(LoginRequest login)**: Authenticates with SAP Business One Service Layer
  - Parameters: Host, CompanyDB, Password, UserName
  - Returns: LoginResp object containing session information
  - Handles SSL certificate validation and cookie management

- **LogoutSAP(LoginResp loginSapInfo)**: Logs out from the current SAP session
  - Parameters: LoginResp object containing session details
  - Returns: Response indicating success/failure

### User-Defined Fields (UDF)
- **AddUDF(LoginResp loginSapInfo, List<UDFEntity> udflist)**: Adds user-defined fields to SAP
  - Parameters: 
    - LoginResp: Session information
    - List of UDFEntity objects containing field definitions
  - Returns: List of responses for each UDF operation

### SQL Query Management
- **AddQuery(LoginResp login, string sqlName, string sql)**: Manages SQL queries in SAP
  - Parameters:
    - LoginResp: Session information
    - sqlName: Name of the SQL query
    - sql: SQL query text
  - Returns: Response indicating success/failure

### View Management
- **ViewExpose(LoginResp login)**: Manages SQL view exposure in SAP
  - Parameters: LoginResp object containing session information
  - Returns: Response indicating success/failure

### General Service Layer Requests
- **SLRequest(LoginResp loginSapInfo, string serviceLayerUrl, HttpMethod method, object obj)**: Generic method for making service layer requests
  - Parameters:
    - LoginResp: Session information
    - serviceLayerUrl: Target URL
    - method: HTTP method (GET, POST, etc.)
    - obj: Optional request body object
  - Returns: Response with success/failure status

## Entity Models

### LoginRequest
- Host: Service Layer host URL
- CompanyDB: Company database name
- Password: User password
- UserName: Username

### LoginResp
- Inherits from LoginRequest
- SessionId: Active session identifier
- RouteId: Route identifier
- Error: Error information if any

### UDFEntity
- TableName: Target table name
- Name: Field name
- Description: Field description
- Type: Field type (e.g., db_Alpha)
- SubType: Field subtype
- Size: Field size
- DefaultValue: Default value
- ValidValuesMD: List of valid values
- Mandatory: Mandatory field indicator

### Response<T>
- Success: Operation success status
- Message: Response message
- Data: Generic response data

## Requirements
- .NET 8.0
- Microsoft.AspNetCore.Mvc.NewtonsoftJson (8.0.11)

## Usage Example
```csharp
// Login to SAP
var loginRequest = new LoginRequest 
{
    Host = "https://your-sap-server:50000",
    CompanyDB = "YourCompanyDB",
    UserName = "manager",
    Password = "your-password"
};

var slGate = new SLGate();
var loginResponse = slGate.LoginSAP(loginRequest);

// Add UDF
var udfList = new List<UDFEntity> 
{
    new UDFEntity 
    {
        TableName = "OITM",
        Name = "CustomField",
        Type = "db_Alpha",
        Size = 50
    }
};

var udfResponse = slGate.AddUDF(loginResponse, udfList);

// Logout
var logoutResponse = slGate.LogoutSAP(loginResponse);
```
