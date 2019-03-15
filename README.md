# DB-Restore-Windows-App
Incredibly Mediocre Windows application to Backup and Restore Databases (Local or network)

# Coding Guidelines

# Git

I prefer a rebase workflow and **No** feature branches. Most work happens directly on the master branch. For that reason, I recommend setting the pull.rebase setting to true.

`git config --global pull.rebase true`

# Indentation
Please, No space, only TABS.

# Names

1. Use PascalCase for type names
2. Use PascalCase for enum values
3. Use camelCase for function and method names
4. Use camelCase for property names and local variables
5. Use whole words in names when possible
   
# Strings

1. Use "double quotes" for strings shown to the user that need to be externalized (localized)
2. Use 'single quotes' otherwise
3. All strings visible to the user need to be externalized
