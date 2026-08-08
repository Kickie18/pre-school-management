# Postman QA Checklist

## Preconditions

1. API is running locally.
2. Base URL is reachable (default: https://localhost:7147).
3. Super admin account exists: superadmin@preschool.local / Admin@123.

## Automated Run (Newman)

```powershell
./scripts/run-postman-qa.ps1 -BaseUrl "https://localhost:7147"
```

Expected result:

1. Process exits with code 0.
2. Report file is created at postman/newman-report.json.
3. All tests in collection pass.

## Collection Runner Sequence

1. Auth/Login Super Admin
2. Auth/Refresh Token
3. QA Seed/Seed Quick QA Data
4. QA Seed/Seed Status
5. Students/Get Students
6. Students/Get Student By Id
7. Attendance/Get Attendances
8. Reports & Dashboard/Dashboard Summary
9. Reports & Dashboard/Attendance Report
10. Reports & Dashboard/Fee Collection Report

## Key Assertions Included

1. Every request asserts HTTP 200.
2. Auth requests assert success and token payload.
3. Seed request asserts entity IDs are populated in collection variables.
4. Student by ID asserts response ID equals saved variable studentId.
5. Dashboard asserts numerical totals are present.
6. Report requests assert array payloads are returned.

## Optional Manual Spot Checks

1. Open Swagger at /swagger and verify authorize flow with bearer token.
2. Verify seeded status counts are non-zero for users, classes, students.
3. Soft delete a record and ensure it does not appear in GET all results.
