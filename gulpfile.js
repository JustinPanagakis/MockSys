import { series } from 'gulp';
import { exec } from 'gulp-execa';

const dataProj = './MockSys.Reporting.Data/MockSys.Reporting.Data.csproj';
const startupProj = './MockSys.Web/MockSys.Web.csproj';

export function addMigration() {
    const name = `${Date.now()}`;
    const cmd = [
        `dotnet ef migrations add ${name}`,
        `--project ${dataProj}`,
        `--startup-project ${startupProj}`,
        '--output-dir Migrations',
        '--context ReportingDbContext'
    ].join(' ');
    return exec(cmd, { stdio: 'inherit' });
}

export function updateDatabase() {
    const cmd = [
        'dotnet ef database update',
        `--project ${dataProj}`,
        `--startup-project ${startupProj}`
    ].join(' ');
    return exec(cmd, { stdio: 'inherit' });
}

export function rollbackMigration() {
    const removeCmd = [
        'dotnet ef migrations remove',
        `--project ${dataProj}`,
        `--startup-project ${startupProj}`
    ].join(' ');
    return exec(removeCmd, { stdio: 'inherit' });
}

export const addMigrationAndUpdateDatabase = series(addMigration, updateDatabase);
