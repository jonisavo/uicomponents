//
// generate_unitypackage.js
//
// This script creates a .unitypackage in the dist folder.
//

const path = require('path');
const fs = require('fs');
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
const outputFolder = path.resolve(distFolder, 'Assets', 'Plugins', 'UIComponents');

try {
    fs.rmSync(distFolder, { recursive: true });
} catch {
	console.log('dist folder does not exist, skipping wipe');
}

fs.mkdirSync(distFolder);
fs.mkdirSync(path.join(distFolder, 'Assets'));
fs.mkdirSync(path.join(distFolder, 'Assets', 'Plugins'));
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

const command = `${unityPackerPath} . ${'UIComponents_' + version}`;

console.log(`Executing command: ${command} in ${distFolder}`);

const output = execSync(command, { cwd: distFolder });

console.log(output.toString());
