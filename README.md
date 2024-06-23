# WinHarden
Program to find misconfigurations on Windows systems

# Introduction
Perform a complete review of a system may be complex:
  - Automatic tools only provide concrete features and do not ease analyzing matters that are not included on them.
  - Executing manually commands to gather missing information of the automatic tools is a tedious task, which it is increased if reviewing security of several hosts is required.
  - Output and features of the public tools may not fit with the requirements and configurations of your organization or workstation.
  - Sometimes, output of .NET libraries does not cover all possible Windows configurations, flags and masks.
  - Some concrete Windows commands provide output in a non-readable way that hinders to analyze them properly. 

# Tool usage
1. Tool has a simple interface. User may select where configuration output commands and filtered results are showed. Moreover, user may select list of folders to retrieve access control and audit confirguration for a more detailed analysis in critical folders.
2. Tool does not require administrator privileges. Nevertheless, executing extracting files module under administrator privileges allows obtaining concrete commands for more results.

# Information gathering module
1. This module generates output files to be reviewed manually, to be processed in next analysis module or to be processed by other scripts/tools of the user.
2. Aim of this module is gathering all information of a Windows system that could be useful and interesting for any security review.
3. Examples of relevant generated files for security reviews:
   - Access control on paths/registry keys related to binary planting attacks to allow escalating privileges, as startup folders, startup programs, running processes, scheduled tasks, services.
   - Access control on net shared folders.
   - System audit policy.
   - Relevant register keys.
   - System privileges for local users.   
   - Password policy.
   - Active directory information for local administrator accounts.
   - Access control and audit configuratoin in selected list of folders by input file.

# Analysis result module
1. This module process files obtained in the previous module.
2. Aim of this module is filtering relevant information to ease identifying any possible misconfiguration that deteriorates the security of the Windows host.
3. Examples of relevant filtered result files for security reviews:
   - Vulnerable folders, tasks, programs, services and registry keys to binary planting and other techniques to escalate privileges. This tool considers as vulnerable ojects when any non-administrator group or user may modifiyng them, instead of only filtering by Everyone or similar groups as other public tools work.
   - Unsecure registry key values.
   - Process arguments to try to find passwords as parameters.
   - Antivirus status.
   - Windows version and latest hotfix to allow assessing the updating status.

# Next steps
1. Gather feedback from the community for improvement.
2. Continue adding more features and tools to build an even more complete Windows auditing framework.
