//
// update_package_version.js
//
// This script updates the version in the package's package.json file,
// as well as all occurrences of it in the README.md file.
//

const path = require('path');
const fs = require('fs');

const args = process.argv.slice(2);

if (args.length === 0) {
    console.error('No argument specified');
    process.exit(1);
}

// Update package.json

const packageJsonPath = path.resolve(__dirname, '..', 'Assets', 'UIComponents', 'package.json');

if (!fs.existsSync(packageJsonPath)) {
    console.error('package.json not found');
    process.exit(1);
}

const packageJson = JSON.parse(fs.readFileSync(packageJsonPath));

const currentVersion = packageJson.version;

packageJson.version = args[0];

const packageJsonString = JSON.stringify(packageJson, null, 4);

fs.writeFileSync(packageJsonPath, packageJsonString);

// Update README.md

const readmePath = path.resolve(__dirname, '..', 'README.md');

if (!fs.existsSync(readmePath)) {
    console.error('README.md not found. Skipping.');
    process.exit(0);
}

const readmeContents = String(fs.readFileSync(readmePath));

const modifiedReadmeContents = readmeContents
    .replaceAll(currentVersion, args[0]);

fs.writeFileSync(readmePath, modifiedReadmeContents);
