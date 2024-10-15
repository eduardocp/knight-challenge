# Knight Challenge

This project is a Knight management system that includes a backend API and a frontend application. The backend is built with .NET and MongoDB, while the frontend is built with Vue.js.

# Features
- List all knights
- View knight details
- Add a new knight
- Update an existing knight
- Delete a knight

## Prerequisites

- Docker
- Docker Compose

## Getting Started

### Running the Application via Docker Compose

1. Clone the repository:
    ```sh
    git clone <repository-url>
    cd <repository-directory>
    ```

2. Build and run the application using Docker Compose:
    ```sh
    docker-compose up --build
    ```

   This will start the following services:
   - MongoDB
   - Knight API
   - Knight Frontend

### Hosted URL's

The API is hosted at `https://localhost:8081`.

The frontend application is hosted at `http://localhost:8090`.

### API Endpoints

#### Get All Knights

- **URL:** `/knights`
- **Method:** `GET`
- **Query Parameters:**
  - `filter` (optional): If set to `heroes`, only knights older than 7 years will be returned.
- **Response:** `200 OK` with a list of knights.

#### Get Knight by ID

- **URL:** `/knights/{id}`
- **Method:** `GET`
- **Response:** `200 OK` with the knight details.

#### Add a New Knight

- **URL:** `/knights`
- **Method:** `POST`
- **Body:**
  ```json
  {
    "name": "string",
    "nickName": "string",
    "birthday": "string",
    "weapons": ["string"],
    "attributes": {
      "strength": 0,
      "dexterity": 0,
      "constitution": 0,
      "intelligence": 0,
      "wisdom": 0,
      "charisma": 0
    },
    "keyAttribute": "string"
  }
  ```
- **Response:** 200 OK with the created knight details.

#### Update a Knight

- **URL:** `/knights/{id}`
- **Method:** `PATCH`
- **Body:**
    ```json
    {
    "name": "string",
    "nickName": "string",
    "birthday": "string",
    "weapons": ["string"],
    "attributes": {
        "strength": 0,
        "dexterity": 0,
        "constitution": 0,
        "intelligence": 0,
        "wisdom": 0,
        "charisma": 0
    },
    "keyAttribute": "string"
    }
    ```
- **Response:** 200 OK with the updated knight details.

#### Delete a Knight

- **URL:** `/knights/{id}`
- **Method:** `DELETE`
- **Body:** `200 OK`

### Running individually

#### Backend
The backend code is located in the `api` directory. To run the backend locally:

1. Navigate to the api directory:

    ```bash
    cd api
    ```

2. Run the backend:

    ```bash
    dotnet run
    ```

The backend API is hosted at https://localhost:8081

It's also deployed a API explorer ([Scalar](https://www.scalar.com)) at https://localhost:8081/scalar/v1

#### Frontend
The frontend code is located in the `front` directory. To run the frontend locally:

1. Navigate to the front directory:

    ```bash
    cd front
    ```

2. Install dependencies:

    ```bash
    npm install
    ```

3. Run the frontend:

    ```bash
    npm run dev
    ```

The frontend application is hosted at http://localhost:5173.
