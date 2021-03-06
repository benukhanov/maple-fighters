# Auth Service

The authentication service stores user data and verifies user.

## Project Structure

```
├── Authenticator.API
│   ├── Constants
│   ├── Controllers
│   ├── Converters
│   ├── Datas
│   ├── Properties
│   └── Validators
├── Authenticator.Domain
│   └── Aggregates
│       └── User
│           └── Services
├── Authenticator.Infrastructure
│   └── Repository
└── Authenticator.UnitTests
    ├── API
    │   └── Controllers
    └── Domain
        └── User
            └── Services
```

## Quickstart

### Docker

Follow these instructions to create an image and run a container:

1. Build a docker image:

```bash
make build
```

2. Running in a docker container:

```bash
make run
```

You should now be able to access it at `http://localhost:50050`.
