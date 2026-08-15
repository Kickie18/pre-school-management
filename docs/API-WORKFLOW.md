# Preschool Management API Workflow

## Base URL

```text
http://localhost:5175
```

Swagger UI:

```text
http://localhost:5175/swagger
```

Scalar:

```text
http://localhost:5175/scalar
```

All school, teacher, parent, student, and classroom endpoints require a bearer token.

## 1. Login

```http
POST /api/Auth/login
Content-Type: application/json
```

Use the seeded Super Admin account:

```json
{
  "email": "superadmin@preschool.local",
  "password": "Admin@123"
}
```

The login response contains `data.accessToken`. Send that token on protected requests:

```http
Authorization: Bearer YOUR_ACCESS_TOKEN
```

## 2. Endpoint Summary

| Resource | Create | Retrieve all | Retrieve one | Update | Delete |
|---|---|---|---|---|---|
| Schools | `POST /api/Schools` | `GET /api/Schools` | `GET /api/Schools/{id}` | `PUT /api/Schools/{id}` | `DELETE /api/Schools/{id}` |
| Teachers | `POST /api/Teachers` | `GET /api/Teachers` | `GET /api/Teachers/{id}` | `PUT /api/Teachers/{id}` | `DELETE /api/Teachers/{id}` |
| Users | `POST /api/Users` | `GET /api/Users` | `GET /api/Users/{id}` | `PUT /api/Users/{id}` | `DELETE /api/Users/{id}` |
| Parents | `POST /api/Parents` | `GET /api/Parents` | `GET /api/Parents/{id}` | `PUT /api/Parents/{id}` | `DELETE /api/Parents/{id}` |
| Classrooms | `POST /api/ClassRooms` | `GET /api/ClassRooms` | `GET /api/ClassRooms/{id}` | `PUT /api/ClassRooms/{id}` | `DELETE /api/ClassRooms/{id}` |
| Students | `POST /api/Students` | `GET /api/Students` | `GET /api/Students/{id}` | `PUT /api/Students/{id}` | `DELETE /api/Students/{id}` |

The classroom route is `/api/ClassRooms` because the controller is named `ClassRoomsController`.

## 3. Recommended Creation Order

Create records in this order:

1. Login and obtain the access token.
2. Create a school.
3. Create a teacher linked to the school.
4. Create a classroom, optionally linked to the teacher.
5. Create a Parent-role user account.
6. Create the parent profile linked to that user.
7. Create a student linked to the classroom and parent.
8. Retrieve the created records using their returned IDs.

Relationships:

```text
School
  └── Teachers
        └── ClassRooms
              └── Students
                    └── Parent
                          └── User login account
```

## 4. Create a School

Only `SuperAdmin` and `PreschoolAdmin` can create schools.

```http
POST /api/Schools
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json
```

```json
{
  "schoolName": "Little Stars Preschool",
  "address": {
    "addressLine1": "123 Main Street",
    "addressLine2": null,
    "city": "Springfield",
    "state": "Demo State",
    "postalCode": "00000",
    "country": "Demo Country"
  },
  "contactNumber": "+1-555-0100",
  "email": "admin@littlestars.local",
  "logo": "https://example.com/logos/little-stars.png"
}
```

### Required school fields

| Field | Required | Description |
|---|---:|---|
| `schoolName` | Yes | School name |
| `address` | Yes | Required address object stored in the `Addresses` table |
| `contactNumber` | Yes | School contact number |
| `email` | Yes | School email |
| `logo` | No | Logo URL or file reference |

Save the returned `data.id` as `SCHOOL_ID`.

## 5. Create a Teacher

Only `SuperAdmin` and `PreschoolAdmin` can create teachers.

```http
POST /api/Teachers
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json
```

```json
{
  "employeeCode": "TCH-001",
  "firstName": "Sarah",
  "lastName": "Johnson",
  "gender": 2,
  "dob": "1990-05-15T00:00:00Z",
  "qualification": "Bachelor of Education",
  "experience": 8,
  "phoneNumber": "+1-555-0101",
  "email": "sarah.johnson@littlestars.local",
  "joiningDate": "2026-08-15T00:00:00Z",
  "profileImage": null,
  "schoolId": "SCHOOL_ID",
  "userId": "TEACHER_USER_ID",
  "address": {
    "addressLine1": "21 Teacher Lane",
    "addressLine2": null,
    "city": "Springfield",
    "state": "Demo State",
    "postalCode": "00000",
    "country": "Demo Country"
  }
}
```

### Required teacher fields

| Field | Required | Description |
|---|---:|---|
| `employeeCode` | Yes | Unique employee code |
| `firstName` | Yes | Teacher first name |
| `lastName` | Yes | Teacher last name |
| `gender` | Yes | `1` Male, `2` Female, `3` Other |
| `dob` | Yes | Date of birth |
| `qualification` | Yes | Qualification |
| `experience` | Yes | Years of experience |
| `phoneNumber` | Yes | Contact number |
| `email` | Yes | Teacher email |
| `joiningDate` | Yes | Joining date |
| `schoolId` | Yes | ID returned from school creation |
| `userId` | Yes | ID of a User created with the `Teacher` role |
| `profileImage` | No | Image URL or file reference |

Teacher address is required and is stored in `Addresses`; only `addressId` is stored in `Teachers`. Create the teacher's login user first through `POST /api/Users` with the `Teacher` role, then use the returned ID as `userId` here.

Save the returned `data.id` as `TEACHER_ID`.

## 6. Create a Classroom

Only `SuperAdmin`, `PreschoolAdmin`, and `Teacher` can create classrooms.

```http
POST /api/ClassRooms
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json
```

```json
{
  "className": "Sunshine Group",
  "ageGroup": "3-4 years",
  "capacity": 20,
  "schoolId": "SCHOOL_ID",
  "teacherId": "TEACHER_ID"
}
```

### Required classroom fields

| Field | Required | Description |
|---|---:|---|
| `className` | Yes | Unique classroom name |
| `ageGroup` | Yes | Age range or group description |
| `capacity` | Yes | Maximum number of students |
| `schoolId` | Yes | ID of the school that owns the classroom |
| `teacherId` | No | Assigned teacher ID |

`teacherId` can be `null` when the classroom is not assigned yet:

```json
{
  "className": "Rainbow Group",
  "ageGroup": "4-5 years",
  "capacity": 20,
  "schoolId": "SCHOOL_ID",
  "teacherId": null
}
```

Save the returned `data.id` as `CLASSROOM_ID`.

The current classroom model has no address field. Classroom location information cannot be stored without extending the model.

## 7. Create a Parent User Account

A parent profile references a `User` through `userId`. Create the login user first.

Retrieve the available roles:

```http
GET /api/Roles
Authorization: Bearer YOUR_ACCESS_TOKEN
```

Find the role whose `roleName` is `Parent` and save its ID as `PARENT_ROLE_ID`.

Create the user:

```http
POST /api/Users
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json
```

```json
{
  "firstName": "Michael",
  "lastName": "Brown",
  "email": "michael.brown@example.com",
  "phoneNumber": "+1-555-0102",
  "password": "Parent@123",
  "roleId": "PARENT_ROLE_ID",
  "profilePicture": null
}
```

### Required user fields

| Field | Required | Description |
|---|---:|---|
| `firstName` | Yes | User first name |
| `lastName` | Yes | User last name |
| `email` | Yes | Unique login email |
| `phoneNumber` | Yes | Phone number |
| `password` | Yes | Initial login password |
| `roleId` | Yes | Must be the `Parent` role ID |
| `profilePicture` | No | Image URL or file reference |

Save the returned `data.id` as `PARENT_USER_ID`.

## 8. Create the Parent Profile

Only `SuperAdmin` and `PreschoolAdmin` can create parent profiles.

```http
POST /api/Parents
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json
```

```json
{
  "fatherName": "Michael Brown",
  "motherName": "Emily Brown",
  "email": "michael.brown@example.com",
  "phoneNumber": "+1-555-0102",
  "address": "45 Oak Avenue, Springfield",
  "occupation": "Software Engineer",
  "userId": "PARENT_USER_ID",
  "schoolId": "SCHOOL_ID"
}
```

### Required parent fields

| Field | Required | Description |
|---|---:|---|
| `fatherName` | Yes | Father's full name |
| `motherName` | Yes | Mother's full name |
| `email` | Yes | Parent profile email |
| `phoneNumber` | Yes | Parent phone number |
| `address` | Yes | Parent residential address as plain text |
| `occupation` | Yes | Parent occupation |
| `userId` | Yes | ID of the parent login user |
| `schoolId` | Yes | ID of the school that owns the parent record |

Save the returned `data.id` as `PARENT_ID`.

The parent address is stored in the parent profile's `address` field as plain text. Parent addresses are not part of the normalized `Addresses` table change.

## 9. Create a Student

Only `SuperAdmin`, `PreschoolAdmin`, and `Teacher` can create students.

```http
POST /api/Students
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json
```

```json
{
  "admissionNumber": "STU-2026-001",
  "rollNumber": "ROLL-001",
  "firstName": "Emma",
  "lastName": "Brown",
  "gender": 2,
  "dob": "2022-03-10T00:00:00Z",
  "bloodGroup": "O+",
  "address": {
    "addressLine1": "45 Oak Avenue",
    "addressLine2": null,
    "city": "Springfield",
    "state": "Demo State",
    "postalCode": "00000",
    "country": "Demo Country"
  },
  "joiningDate": "2026-08-15T00:00:00Z",
  "classId": "CLASSROOM_ID",
  "parentId": "PARENT_ID",
  "schoolId": "SCHOOL_ID",
  "profilePicture": null,
  "status": 1
}
```

### Required student fields

| Field | Required | Description |
|---|---:|---|
| `admissionNumber` | Yes | Unique student admission number |
| `rollNumber` | No | Optional student roll number |
| `firstName` | Yes | Student first name |
| `lastName` | Yes | Student last name |
| `gender` | Yes | `1` Male, `2` Female, `3` Other |
| `dob` | Yes | Date of birth |
| `bloodGroup` | Yes | Blood group |
| `address` | Yes | Required address object stored in the `Addresses` table |
| `joiningDate` | Yes | Joining date |
| `classId` | Yes | Classroom ID |
| `parentId` | Yes | Parent profile ID |
| `schoolId` | Yes | ID of the school that owns the student record |
| `profilePicture` | No | Image URL or file reference |
| `status` | Yes | `1` Active, `2` Transferred, `3` Inactive |

The student address is stored in `Addresses`; only `addressId` is stored in `Students` and it does not automatically inherit the parent address.

## 10. Retrieve Records

Retrieve all records:

```http
GET /api/Schools
GET /api/Teachers
GET /api/ClassRooms
GET /api/Parents
GET /api/Students
```

Retrieve one record by ID:

```http
GET /api/Schools/SCHOOL_ID
GET /api/Teachers/TEACHER_ID
GET /api/ClassRooms/CLASSROOM_ID
GET /api/Parents/PARENT_ID
GET /api/Students/STUDENT_ID
```

Every request requires:

```http
Authorization: Bearer YOUR_ACCESS_TOKEN
```

## 11. Pagination, Search, Sorting, and Filtering

List endpoints support these query parameters:

| Parameter | Default | Example |
|---|---:|---|
| `pageNumber` | `1` | `pageNumber=1` |
| `pageSize` | `20` | `pageSize=50` |
| `search` | Empty | `search=Emma` |
| `sortBy` | Empty | `sortBy=firstName` |
| `sortDescending` | `false` | `sortDescending=true` |
| `filterBy` | Empty | `filterBy=status` |
| `filterValue` | Empty | `filterValue=1` |

Example:

```http
GET /api/Students?pageNumber=1&pageSize=20&search=Emma&sortBy=firstName&sortDescending=false
```

Typical response:

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "items": [],
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 0,
    "totalPages": 0
  },
  "errors": []
}
```

## 12. Update Records

Updates use `PUT` and require the complete update DTO. Partial updates are not supported.

Example:

```http
PUT /api/Students/STUDENT_ID
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json
```

```json
{
  "admissionNumber": "STU-2026-001",
  "rollNumber": "ROLL-001",
  "firstName": "Emma",
  "lastName": "Brown",
  "gender": 2,
  "dob": "2022-03-10T00:00:00Z",
  "bloodGroup": "O+",
  "address": {
    "addressLine1": "90 Maple Road",
    "addressLine2": null,
    "city": "Springfield",
    "state": "Demo State",
    "postalCode": "00000",
    "country": "Demo Country"
  },
  "joiningDate": "2026-08-15T00:00:00Z",
  "classId": "CLASSROOM_ID",
  "parentId": "PARENT_ID",
  "schoolId": "SCHOOL_ID",
  "profilePicture": null,
  "status": 1
}
```

The same `PUT /api/{Resource}/{id}` pattern applies to schools, teachers, users, parents, classrooms, and students.

## 13. Delete Records

Delete is implemented as a soft delete:

```http
DELETE /api/Students/STUDENT_ID
Authorization: Bearer YOUR_ACCESS_TOKEN
```

Typical response:

```json
{
  "success": true,
  "message": "Deleted successfully",
  "data": {
    "id": "STUDENT_ID"
  },
  "errors": []
}
```

## Important Constraints

- School, teacher, and student addresses are stored in the `Addresses` table.
- `Schools.AddressId`, `Teachers.AddressId`, and `Students.AddressId` are the only address values stored on those owner tables.
- Parent address remains `Parent.address`; it was not changed by this request.
- Address creation is mandatory for school, teacher, and student create/update requests.
- Address fields are `addressLine1`, optional `addressLine2`, `city`, `state`, `postalCode`, and `country`.
- Classroom currently has no address property.
- `EmployeeCode` must be unique.
- `AdmissionNumber` must be unique.
- `RollNumber` is optional; when supplied, it is stored in `Students.RollNumber`.
- `ClassName` must be unique.
- User email must be unique.
- A teacher must reference an existing `schoolId`.
- A teacher must reference an existing `userId` whose role is `Teacher`.
- A classroom must reference an existing `schoolId`; `teacherId` remains optional.
- A student must reference an existing `classId` and `parentId`.
- A parent must reference an existing `userId`.
- A parent must reference an existing `schoolId`.
- A student must reference an existing `schoolId` consistent with its classroom.
- Use numeric enum values in JSON: `Gender` is `1` Male, `2` Female, `3` Other; `StudentStatus` is `1` Active, `2` Transferred, `3` Inactive.
- API responses use the structure `{ success, message, data, errors }`.

For an existing database, run [database/address-migration.sql](../database/address-migration.sql) before starting the updated API. It creates and backfills the address table, adds foreign keys, makes the three address IDs mandatory, and removes the old school/student text address columns.

Then run [database/teacher-classroom-migration.sql](../database/teacher-classroom-migration.sql). It links existing teachers to matching Teacher-role users and classrooms to their assigned teacher's school, then makes `Teachers.UserId` and `ClassRooms.SchoolId` mandatory.

Then run [database/parent-student-school-migration.sql](../database/parent-student-school-migration.sql). It backfills student schools from classrooms and parent schools from their students, then makes `Parents.SchoolId` and `Students.SchoolId` mandatory.

Run [database/student-roll-number-migration.sql](../database/student-roll-number-migration.sql) to add the nullable `Students.RollNumber` column to an existing database.
