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

// Update other files

function replaceVersionInFile(filePath) {
    if (!fs.existsSync(filePath)) {
        console.error(`${filePath} not found. Skipping.`);
        return;
    }
    
    const fileString = fs.readFileSync(filePath).toString();

    const newFileString = fileString.replace(new RegExp(currentVersion, 'g'), args[0]);

    fs.writeFileSync(filePath, newFileString);
}

const readmePath = path.resolve(__dirname, '..', 'README.md');
const pathsToReplace = [readmePath];

for (const path of pathsToReplace) {
    replaceVersionInFile(path);
}
