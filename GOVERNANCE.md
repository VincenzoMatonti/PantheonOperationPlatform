# Pantheon Governance

## Overview

Pantheon is an open source project developed and maintained by **Vincenzo Matonti**.

The project welcomes community contributions while maintaining a controlled development, review, and release process.

This document describes the current governance model of the project.

## Project Maintainer

The project maintainer is responsible for:

* maintaining the official repository;
* reviewing and integrating contributions;
* managing the development workflow;
* maintaining the technical direction of the project;
* managing repository access and security;
* approving official releases;
* making final decisions regarding the integration of contributions.

As Pantheon grows, additional maintainers or governance roles may be introduced.

## Community Contributions

Community contributions are welcome.

External contributors can participate without direct write access to the official repository by working through forks and pull requests.

The contribution flow is:

```text
Fork
  ↓
Contributor branch
  ↓
Pull Request → community
  ↓
Maintainer review
  ↓
Maintainer integration
```

The `community` branch is the controlled entry point for external contributions.

For contribution requirements and development guidelines, see [`CONTRIBUTING.md`](CONTRIBUTING.md).

## Development Flow

Pantheon uses a controlled branch workflow:

```text
community
    ↓
feature/*
    ↓
feature
    ↓
dev
    ↓
stage
    ↓
main
    ↓
Release
```

| Branch      | Purpose                       |
| ----------- | ----------------------------- |
| `community` | External contribution intake  |
| `feature`   | Internal feature integration  |
| `dev`       | Development integration       |
| `stage`     | Pre-production validation     |
| `main`      | Production and release source |

Temporary development branches should follow the naming conventions defined in `CONTRIBUTING.md`.

## Pull Requests

Changes to the project are introduced through pull requests.

External contributors must target the `community` branch.

Internal development follows the controlled branch workflow defined above.

A pull request may be:

* reviewed;
* approved;
* modified;
* rejected;
* integrated into the project.

Opening a pull request does not guarantee that the proposed change will be accepted.

## Decision Making

Technical and project decisions are currently made by the project maintainer.

Community feedback may be considered through:

* Issues;
* Discussions;
* Pull Requests;
* community contributions;
* technical proposals.

The maintainer has final responsibility for decisions affecting the official project.

As the project grows, the decision-making model may evolve to include additional maintainers or community roles.

## Repository Access

Direct write and administrative access to the official repository is controlled by the project maintainer.

Contributors without direct repository access should use forks and pull requests.

Repository permissions, branch protection, security controls, and release controls are managed separately from the project's open source license.

## Releases

Official releases are created from the controlled production source.

The project maintainer is currently responsible for:

* approving releases;
* creating release tags;
* publishing official releases;
* maintaining supported release information.

Release procedures may evolve as the project grows.

## Governance Evolution

The current governance model is intentionally simple.

As Pantheon develops a larger community and ecosystem, governance may evolve to introduce:

* additional maintainers;
* technical reviewers;
* community roles;
* formal decision-making processes;
* contributor agreements;
* partner or ecosystem programs.

Changes to the governance model should be documented and communicated through the project's normal contribution and discussion channels.
