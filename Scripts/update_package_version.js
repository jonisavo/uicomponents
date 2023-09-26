//
// update_package_version.js
//
// This script updates the version in the package's package.json file,
// as well as all occurrences of it in the README.md file and Roslyn projects.
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
const roslynSolutionPath = path.resolve(__dirname, '..', 'UIComponents.Roslyn', 'UIComponents.Roslyn.sln');

/**
 * @param {string} startDir
 * @returns {string[]}
 */
function findCsprojFiles(currentDir) {
    let csprojFiles = [];
    const files = fs.readdirSync(currentDir);

    for (const file of files) {
        const filePath = path.join(currentDir, file);
        const isDirectory = fs.statSync(filePath).isDirectory();

        if (isDirectory) {
            csprojFiles = csprojFiles.concat(findCsprojFiles(filePath));
        } else if (file.endsWith('.csproj')) {
            csprojFiles.push(filePath);
        }
    }

    return csprojFiles;
}

const roslynProjectPaths = findCsprojFiles(path.resolve(__dirname, '..', 'UIComponents.Roslyn'));
const pathsToReplace = [readmePath, roslynSolutionPath, ...roslynProjectPaths];

for (const path of pathsToReplace) {
    replaceVersionInFile(path);
}
