//
// update_snapshot_versions.js
//
// This script updates the version in all UIComponents.Roslyn snapshot
// test files, as well as its Constants.cs file.
//

const path = require('path');
const fs = require('fs');

const args = process.argv.slice(2);

if (args.length === 0) {
    console.error('No argument specified');
    process.exit(1);
}

const roslynProjectPath = path.resolve(__dirname, '..', 'UIComponents.Roslyn');
const roslynConstantsPath = path.resolve(
    roslynProjectPath,
    'UIComponents.Roslyn.Generation',
    'Constants.cs'
);
const roslynSnapshotsFolderPath = path.resolve(
    roslynProjectPath,
    'UIComponents.Roslyn.Generation.Tests',
    'Snapshots'
);

function replaceVersionInFile(filePath) {
    if (!fs.existsSync(filePath)) {
        console.error(`${filePath} not found. Skipping.`);
        return;
    }

    const fileString = fs.readFileSync(filePath).toString();

    const newFileString = fileString.replace(new RegExp(currentVersion, 'g'), args[0]);

    fs.writeFileSync(filePath, newFileString);
}

const pathsToReplace = [roslynConstantsPath];

const verifiedSnapshotFiles = fs.readdirSync(roslynSnapshotsFolderPath);

for (const file of verifiedSnapshotFiles) {
    if (file.endsWith('.verified.cs')) {
        pathsToReplace.push(path.resolve(roslynSnapshotsFolderPath, file));
    }
}

for (const path of pathsToReplace) {
    replaceVersionInFile(path);
}
