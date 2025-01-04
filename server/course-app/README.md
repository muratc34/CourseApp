# Backend

## Run Backend
### 1. Clone repository
```bash
git clone https://github.com/muratc34/CourseApp.git
cd .\CourseApp\server\course-app\
````
### 2. Update database connection string to yours
```json
"ConnectionStrings": {
  "Database": "Server=localhost;Port=5432;Database=CourseApp;Search Path=public;User Id={your_user};Password={your_password}"
}
````
### 3. Run docker compose
```bash
docker-compose up -d
````
### 4. Run API
```bash
cd .\src\API\
dotnet run
````

Congratulations 🎉
Go to the this url on your browser: [https://localhost:5050/swagger/index.html](https://localhost:5050/swagger/index.html)


## API Endpoints
Note: All ids must be guid

### **Authentication**
- **POST /api/Authentication/Login**: User login<br/>
Request Body:
```json
{
  "email": "string",
  "password": "string"
}
```
- **PATCH /api/Authentication/ChangePassword/{userId}**: User password change<br/>
Request Body:
```json
{
  "oldPassword": "string",
  "newPassword": "string"
}
```
- **POST /api/Authentication/CreateTokenByRefreshToken**: Create access token with refresh token<br/>
Request Body:
```json
{
  "token": "string"
}
```
- **POST /api/Authentication/ExterminateRefreshToken**: Exterminate refresh token<br/>
Request Body:
```json
{
  "token": "string"
}
```
- **POST /api/Authentication/ConfirmEmail**: Confirm user email<br/>
Request Body:
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "token": "string"
}
```
- **POST /api/Authentication/ResendEmailConfirmationToken/{userId}**: Resend email confirmation token<br/>

### **Categories**
- **POST /api/Categories**: Create a category<br/>
Request Body:
```json
{
  "name": "string"
}
```
- **GET /api/Categories**: Get categories<br/>
- **PUT /api/Categories/{categoryId}**: Update category by id<br/>
Request Body:
```json
{
  "name": "string"
}
```
- **DELETE /api/Categories/{categoryId}**: Delete category by id<br/>
- **GET /api/Categories/{categoryId}**: Get category by id<br/>

### **Courses**
- **POST /api/Courses**: Create a course<br/>
Request Body:
```json
{
  "name": "string",
  "description": "string",
  "price": 0,
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "instructorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```
- **GET /api/Courses?pageIndex={pageIndex}&pageSize={pageSize}**: Get courses by pageSize and pageIndex<br/>
- **PUT /api/Courses**: Update course by id<br/>
Request Body:
```json
{
  "name": "string",
  "description": "string",
  "price": 0,
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "instructorId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```
- **DELETE /api/Courses**: Delete course by id<br/>
- **GET /api/Courses/{courseId}**: Get course by id<br/>
- **GET /api/Courses/Categories/{categoryId}?pageIndex={pageIndex}&pageSize={pageSize}**: Get courses by categoryId, pageSize and pageIndex<br/>
- **GET /api/Courses/Users/{userId}**: Get courses by enrolled userId<br/>
- **GET /api/Courses/Instructor/{userId}**: Get courses by instructor userId<br/>
- **POST /api/Courses/UploadImage/{courseId}**: Upload course image by id<br/>
```json
multipart/form-data
{
  "formFile": IFormFile
}
```
- **DELETE /api/Courses/UploadImage/{courseId}**: Delete course image by id<br/>
- **GET /api/Courses/Search**: Search course by name<br/>


### **Orders**
- **POST /api/Orders**: Create an order<br/>
Request Body:
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "courseIds": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ],
  "city": "string",
  "country": "string",
  "address": "string",
  "zipCode": "string",
  "tcNo": "string"
}
```
- **DELETE /api/Orders/{orderId}**: Delete order by id<br/>
- **GET /api/Orders/{orderId}**: Get order by id<br/>
- **GET /api/Orders/Users/{userId}?pageIndex={pageIndex}&pageSize={pageSize}**: Get orders by userId, pageIndex and pageSize<br/>

### **Payments**
- **POST /api/Payments**: Create a payment<br/>
Request Body:
```json
{
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```
- **POST /api/Payments/Callback**: Taking token from iyzico callback request and send frontend endpoint<br/>
- **POST /api/Payments/CheckoutConfirm**: Confirm iyzico payment and enroll user to ordered courses<br/>
Request Body:
```json
{
  "token": "string"
}
```

### **Roles**
- **GET /api/Roles**: Get roles<br/>
- **POST /api/Roles**: Create a role<br/>
Request Body:
```json
{
  "name": "string"
}
```
- **DELETE /api/Roles/{roleId}**: Delete role by id<br/>
- **GET /api/Roles/{roleId}**: Get role by id<br/>

### **Users**
- **POST /api/Users/Register**: Create a user<br/>
Request Body:
```json
{
  "firstName": "string",
  "lastName": "string",
  "userName": "string",
  "email": "string",
  "password": "string"
}
```
- **GET /api/Users/{userId}**: Get user by id<br/>
- **PUT /api/Users/{userId}**: Update user by id<br/>
Request Body:
```json
{
  "firstName": "string",
  "lastName": "string",
  "userName": "string",
  "email": "string"
}
```
- **DELETE /api/Users/{userId}**: Delete user by id<br/>
- **PUT /api/Users/{userId}/role/{roleId}**: Add role to user<br/>
- **DELETE /api/Users/{userId}/role/{roleId}**: Remove role to user<br/>
- **POST /api/Users/UploadImage/{userId}**: Upload user image by id<br/>
```json
multipart/form-data
{
  "formFile": IFormFile
}
```
- **DELETE /api/Users/UploadImage/{userId}**: Delete user image by id<br/>
