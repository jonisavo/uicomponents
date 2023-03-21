//
// generate_unitypackage.js [version]
//
// This script creates .unitypackage files in the dist folder with the given version.
//

const path = require('path');
const fs = require('fs');
const os = require('os');
const execSync = require('child_process').execSync;

const args = process.argv.slice(2);

if (args.length === 0) {
    console.error('No argument specified');
    process.exit(1);
}

const version = args[0];

const rootFolder = path.resolve(__dirname, '..');
const samplesFolder = path.resolve(rootFolder, 'Assets', 'Samples');
const uiComponentsFolder = path.resolve(rootFolder, 'Assets', 'UIComponents');
const distFolder = path.resolve(rootFolder, 'dist');
const outputPluginsFolder = path.resolve(distFolder, 'Plugins');
const outputFolder = path.resolve(outputPluginsFolder, 'UIComponents');
const roslynPackageFolder = path.resolve(
    rootFolder, 'Library', 'PackageCache',
    'com.unity.roslyn@0.2.2-preview'
);

const dsStoreFile = path.join(rootFolder, '.DS_Store');
if (fs.existsSync(dsStoreFile)) {
    fs.rmSync(dsStoreFile);
}

try {
    fs.rmSync(distFolder, { recursive: true });
} catch {
	console.log('dist folder does not exist, skipping wipe');
}

fs.mkdirSync(distFolder);
fs.mkdirSync(outputPluginsFolder);
fs.mkdirSync(outputFolder); 

fs.cpSync(uiComponentsFolder, outputFolder, { recursive: true });
fs.cpSync(samplesFolder, path.join(outputFolder, 'Samples'), { recursive: true });

const samplesFiles = fs.readdirSync(path.join(outputFolder, 'Samples'));

for (const file of samplesFiles)
{
    if (file.endsWith('.meta')) {
        fs.rmSync(path.join(outputFolder, 'Samples', file));
    }
}

const files = fs.readdirSync(rootFolder);

for (const file of files) {
    if (file.includes('.md')) {
        fs.cpSync(file, path.join(outputFolder, file));
    }
}

const unityPackerPath = path.join(rootFolder, 'Scripts', 'UnityPacker.exe');

function createUnityPackerCommand(packageName) {
    let cmd = `${unityPackerPath} . ${packageName} Assets/ unitypackage`;

    if (os.platform() !== 'win32') {
        cmd = 'mono ' + cmd;
    }
    
    return cmd;
}

const command = createUnityPackerCommand('UIComponents_' + version);

function executeCommand(command) {
    console.log(`Executing command: ${command} in ${distFolder}`);

    const output = execSync(command, { cwd: distFolder });

    console.log(output.toString()); 
}

executeCommand(command);

// Uncomment this section to include com.unity.roslyn in the package, if it exists.
// fs.mkdirSync(path.join(outputPluginsFolder, 'com.unity.roslyn'));
// s.cpSync(roslynPackageFolder, path.join(outputPluginsFolder, 'com.unity.roslyn'), { recursive: true });
// 
// const secondCommand = createUnityPackerCommand('UIComponents_' + version + '_with_roslyn');;
// 
// executeCommand(secondCommand);
