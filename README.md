# ClimaSense API

# Project Description
This project is an ASP.NET Core API application designed to generate a 7-day weather forecast and estimate the energy produced by photovoltaic installations, by default based on the user's current location, or by selecting a point on the map or providing coordinates.

---

## Features
- **7-day Weather Forecast** including:
  - Minimum and maximum temperatures
  - Estimated sunshine duration
  - Estimated energy generation in kWh (kilowatt hours)
- **Weekly Summary**:
  - Minimum and maximum temperatures for the upcoming week
  - Average sea-level and surface pressure for the location
  - Average sunshine duration

---

## Technologies and librays
- **.NET 8**
- **ASP.NET Core** (Web API)
- **Docker and Docker Compose**
- **Scrutor** - for implementing the **Decorator** design pattern
- **IMemoryCache** - for in-memory caching

---

## Design Patterns and Best Practices
- **Decorator Pattern**: Implemented using the Scrutor library to extend the functionality of the weather service with a caching mechanism.
- **Layered Architecture**: The application is divided into the following layers:
  - **API**: Handles HTTP requests.
  - **Core**: Services and interfaces, application logic.
  - **Common Infrastructure**: Tools and configurations shared across the project.

---

## Installation and Running

### Requirements
- **Docker** and **Docker Compose**
- .NET SDK 8.0 (for local application execution)

### Running the Application in Docker
1. Build the Docker image and then run the container:
   ```bash
   docker-compose up --build
