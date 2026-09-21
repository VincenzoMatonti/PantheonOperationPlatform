# Contributing to Pantheon

Thank you for your interest in contributing to Pantheon.

Pantheon is developed with a controlled contribution workflow designed to keep the project stable while allowing external contributors to propose improvements.

## Contribution Flow

External contributions enter the project through the `community` branch.

The expected flow is:

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

Contributors should not open pull requests directly against:

* `feature`
* `dev`
* `stage`
* `main`

External contributions should target:

```text
community
```

## Forks

If you do not have write access to the Pantheon repository, create a fork and work in your own repository.

You may create and manage branches freely inside your fork.

For example:

```bash
git clone <your-fork>
cd PantheonOperationPlatform

git switch -c my-feature
```

Push your branch to your fork and open a pull request targeting:

```text
VincenzoMatonti/PantheonOperationPlatform:community
```

## Pull Requests

Before opening a pull request:

* Make sure the change is focused and understandable.
* Keep commits related to the proposed change.
* Update documentation when necessary.
* Add or update tests when applicable.
* Verify that the project builds successfully.
* Verify that existing functionality is not unnecessarily affected.
* Do not include secrets, credentials, tokens, or private information.

## Pull Request Target

External contributors should target:

```text
community
```

The `community` branch is the controlled entry point for external contributions.

A maintainer may:

* Request changes.
* Reject the contribution.
* Modify the contribution.
* Extract part of the contribution.
* Integrate the contribution into the internal development flow.

Acceptance into `community` does not automatically mean that the contribution will be released.

## Branches

Pantheon uses the following long-lived branches:

| Branch      | Purpose                      |
| ----------- | ---------------------------- |
| `main`      | Production/release source    |
| `stage`     | Pre-production validation    |
| `dev`       | Development integration      |
| `feature`   | Internal feature integration |
| `community` | External contribution intake |

Temporary development branches should use the following convention:

```text
feature/<name>
```

Examples:

```text
feature/hephaestus-execution
feature/hermes-operation-api
feature/docker-runtime
```

## Commits

Use clear and meaningful commit messages.

A commit should describe the change it introduces.

Examples:

```text
add hephaestus execution handler
fix hermes operation validation
update local docker documentation
refactor route repository
```

Avoid commits such as:

```text
fix
changes
update
test
stuff
```

## Documentation

Changes that affect architecture, configuration, APIs, development workflows, or infrastructure should include the corresponding documentation updates when appropriate.

## Security

Never commit:

* Passwords.
* API keys.
* Access tokens.
* Private keys.
* Connection strings containing credentials.
* Cloud credentials.
* Personal or confidential information.

If you discover a security vulnerability, follow the instructions in [SECURITY.md](SECURITY.md) instead of opening a public issue.

## Review

All external contributions are reviewed before being integrated into the Pantheon development flow.

Opening a pull request does not guarantee acceptance.

The project maintainer has final responsibility for deciding whether and how a contribution is integrated.
