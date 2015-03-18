require 'rake/clean'

@file_version = "1.0.0.0"

MDTOOL = "/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"
APPS = %w{
    MvvmCross_All_Incl_Mac.sln
}

task :default => [:clean, :build]

def mdbuild solution, opts = {}
    sh "'#{MDTOOL}' build \"--configuration:Release|Unified\" '#{solution}'", opts
end

desc "Compile projects"
task :build do
     APPS.each do |sln|
     	       mdbuild sln
     end
end
