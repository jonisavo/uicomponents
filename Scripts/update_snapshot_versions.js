//
// update_snapshot_versions.js
//
// This script updates the version in all UIComponents.Roslyn projects, snapshot
// test files, as well as its Constants.cs file.
//

const path = require('path');
const fs = require('fs');
const execSync = require('child_process').execSync

const args = process.argv.slice(2);

if (args.length === 0) {
    console.error('No argument specified');
    process.exit(1);
}

const packageJsonPath = path.resolve(__dirname, '..', 'Assets', 'UIComponents', 'package.json');

if (!fs.existsSync(packageJsonPath)) {
    console.error('package.json not found');
    process.exit(1);
}

const packageJson = JSON.parse(fs.readFileSync(packageJsonPath));

const currentVersion = packageJson.version;

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

const roslynSolutionPath = path.resolve(__dirname, '..', 'UIComponents.Roslyn', 'UIComponents.Roslyn.sln');
const roslynProjectPaths = findCsprojFiles(path.resolve(__dirname, '..', 'UIComponents.Roslyn'));

const pathsToReplace = [roslynConstantsPath, roslynSolutionPath, ...roslynProjectPaths];

const verifiedSnapshotFiles = fs.readdirSync(roslynSnapshotsFolderPath);

for (const file of verifiedSnapshotFiles) {
    if (file.endsWith('.verified.cs')) {
        pathsToReplace.push(path.resolve(roslynSnapshotsFolderPath, file));
    }
}

for (const path of pathsToReplace) {
    replaceVersionInFile(path);
}

const buildOutput = execSync('dotnet build ../UIComponents.Roslyn/UIComponents.Roslyn.sln -c Release', { cwd: __dirname });

console.log(buildOutput.toString());

const testOutput = execSync('dotnet test ../UIComponents.Roslyn/UIComponents.Roslyn.sln -c Release --no-build --verbosity normal', { cwd: __dirname });

console.log(testOutput.toString());

const roslynAssetPath = path.resolve(__dirname, '..', 'Assets', 'UIComponents', 'Roslyn');

/** @param {string} projectName */
function copyOverDll(projectName, additionalPath = '') {
    const roslynBuildPath = path.join(
        roslynProjectPath,
        additionalPath,
        projectName,
        'bin', 'Release', 'netstandard2.0'
    );

    const dllName = projectName + '.dll';
    const pdbName = projectName + '.pdb';

    fs.copyFileSync(path.join(roslynBuildPath, dllName), path.join(roslynAssetPath, dllName));
    fs.copyFileSync(path.join(roslynBuildPath, pdbName), path.join(roslynAssetPath, pdbName));
}

const generatorName = 'UIComponents.Roslyn.Generation';


copyOverDll('UIComponents.Roslyn.Generation');
copyOverDll('UIComponents.Roslyn.Common');
copyOverDll('UIComponents.Roslyn.Analyzers', 'UIComponents.Roslyn.Analyzers');
copyOverDll('UIComponents.Roslyn.Analyzers.CodeFixes', 'UIComponents.Roslyn.Analyzers');
