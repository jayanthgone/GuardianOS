# GuardianOS API Documentation

## Authentication

### Login

**Endpoint**: `POST /api/auth/login`

**Request**:
```json
{
  "email": "admin@example.com",
  "password": "password123",
  "deviceId": "device-uuid"
}
```

**Response** (200 OK):
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600,
  "user": {
    "id": "user-uuid",
    "email": "admin@example.com",
    "fullName": "Administrator",
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

### Refresh Token

**Endpoint**: `POST /api/auth/refresh`

**Request**:
```json
{
  "refreshToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Response** (200 OK):
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600,
  "user": { /* user data */ }
}
```

## Devices

### Get All Devices

**Endpoint**: `GET /api/devices`

**Authorization**: Required (Bearer token)

**Response** (200 OK):
```json
[
  {
    "id": "device-uuid",
    "deviceId": "hardware-uuid",
    "computerName": "DESKTOP-12345",
    "osVersion": "Windows 11 Pro",
    "isOnline": true,
    "lastOnlineAt": "2024-01-15T10:30:00Z",
    "registeredAt": "2024-01-01T00:00:00Z",
    "protectionEnabled": true
  }
]
```

### Register Device

**Endpoint**: `POST /api/devices/register`

**Request**:
```json
{
  "deviceId": "hardware-uuid",
  "computerName": "DESKTOP-12345",
  "osVersion": "Windows 11 Pro",
  "totalMemoryGb": 16,
  "processorName": "Intel Core i7-12700K"
}
```

**Response** (201 Created):
```json
{
  "id": "device-uuid",
  "deviceId": "hardware-uuid",
  "computerName": "DESKTOP-12345",
  "osVersion": "Windows 11 Pro",
  "isOnline": true,
  "lastOnlineAt": "2024-01-15T10:30:00Z",
  "registeredAt": "2024-01-01T00:00:00Z",
  "protectionEnabled": true
}
```

## Blocked Applications

### Get Blocked Applications

**Endpoint**: `GET /api/devices/{deviceId}/blocked-apps`

**Response** (200 OK):
```json
[
  {
    "id": "app-uuid",
    "applicationName": "TikTok.exe",
    "executablePath": "C:\\Program Files\\TikTok\\TikTok.exe",
    "requiresPassword": false,
    "blockReason": "Social media distraction",
    "createdAt": "2024-01-01T00:00:00Z"
  }
]
```

### Add Blocked Application

**Endpoint**: `POST /api/devices/{deviceId}/blocked-apps`

**Request**:
```json
{
  "applicationName": "TikTok.exe",
  "executablePath": "C:\\Program Files\\TikTok\\TikTok.exe",
  "requiresPassword": false,
  "passwordHash": null,
  "blockReason": "Social media distraction"
}
```

**Response** (201 Created):
```json
{
  "id": "app-uuid",
  "applicationName": "TikTok.exe",
  "executablePath": "C:\\Program Files\\TikTok\\TikTok.exe",
  "requiresPassword": false,
  "blockReason": "Social media distraction",
  "createdAt": "2024-01-15T10:30:00Z"
}
```

## Remote Commands

### Send Command

**Endpoint**: `POST /api/commands`

**Request**:
```json
{
  "commandType": "LockComputer",
  "targetDeviceId": "device-uuid",
  "parameters": {},
  "priority": 9
}
```

**Response** (201 Created):
```json
{
  "commandId": "command-uuid",
  "status": "Pending",
  "result": null,
  "executedAt": "2024-01-15T10:30:00Z"
}
```

### Get Command Status

**Endpoint**: `GET /api/commands/{commandId}`

**Response** (200 OK):
```json
{
  "commandId": "command-uuid",
  "status": "Success",
  "result": "Computer locked successfully",
  "executedAt": "2024-01-15T10:30:05Z"
}
```

## Events/Audit Logs

### Get Audit Events

**Endpoint**: `GET /api/devices/{deviceId}/events?pageNumber=1&pageSize=50&eventType=ApplicationLaunchDenied`

**Response** (200 OK):
```json
[
  {
    "id": "event-uuid",
    "eventType": "ApplicationLaunchDenied",
    "deviceId": "device-uuid",
    "computerName": "DESKTOP-12345",
    "windowsUser": "Administrator",
    "description": "User attempted to launch TikTok.exe",
    "additionalData": {
      "applicationName": "TikTok.exe",
      "processId": 1234
    },
    "timestamp": "2024-01-15T10:30:00Z",
    "isSecurityEvent": true
  }
]
```

## SignalR Hub

### Connection

**Endpoint**: `wss://api.guardianOS.com/hubs/notifications`

**Authentication**: Pass JWT token as query parameter: `?access_token=<token>`

### Subscribe to Device

**Method**: `SubscribeToDevice`

```javascript
connection.invoke("SubscribeToDevice", "device-uuid")
    .catch(err => console.error(err));
```

### Receive Notifications

**Event**: `ReceiveNotification`

```javascript
connection.on("ReceiveNotification", (notification) => {
    console.log("Notification:", notification);
});
```

## Error Responses

### 400 Bad Request
```json
{
  "error": "Invalid request parameters",
  "details": "Email is required"
}
```

### 401 Unauthorized
```json
{
  "error": "Invalid credentials"
}
```

### 403 Forbidden
```json
{
  "error": "You do not have permission to access this resource"
}
```

### 404 Not Found
```json
{
  "error": "Device not found"
}
```

### 500 Internal Server Error
```json
{
  "error": "An unexpected error occurred",
  "traceId": "0HN1GJBLV9QH8:00000001"
}
```
