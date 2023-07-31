1. Document changes in `Maestro/changelog.txt`
2. Update version number in `appveyor.yml` if required (not needed while we're still on the arbitrary 6.0 milestone scheme)
3. Update version numbers in various `*AssemblyInfo.cs` files under `Properties` folder (not needed while we're still on the arbitrary 6.0 milestone scheme)
4. Update version number in `Docs/toc.yml`
5. Update any known issues in `Docs/userguide/known_issues.md`
6. In the `gh-pages` branch, move all files and folders in the root (except for `archive`) into `archive/$milestone` where `$milestone` was the previous "current" version/milestone.
7. Update `Applicable Version` section of `Docs/index.md`. Also add link to the (just moved) old version.
8. Make the new release tag and push it. AppVeyor CI will publish a new GitHub release and upload all required artifacts
    8.1. `git tag <new version>`
    8.2. `git push origin --tags`
9. Tidy up the draft GitHub release and publish it as the new release.