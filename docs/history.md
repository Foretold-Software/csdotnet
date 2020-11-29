[license]: # "Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information."

# Project History

### 11/29/2020 - Release v1.1
Projects updated to new SDK-style csproj files.\
Add support for multi-targeting all currently supported and semi-supported versions of the .Net Framework.\
Add NuGet package creation to regular build process.\
Add support for building projects and automating releases from CI/CD pipelines.\
Add automatic build-time versioning, with support for release and non-release branch builds.\
Customize unit test context assertions with extension methods.

New types:
* OneWayValueConverter
* DisposableCollection

Bug fixes:
* Unit tests failing for CultureInfo retrieval on Windows 10.
* Unit tests failing for SetWorkingDirectory method with SDK-style projects.

### 10/8/2020
Foretold-owned git repo created.\
Project restructured for FS git csproj template, and new repo populated.

### 10/1/2020
Project acquired by Foretold Software

### 9/8/2015
First NuGet package release

### 8/31/2015
Project added to git repo

### 2011
Initial project creation