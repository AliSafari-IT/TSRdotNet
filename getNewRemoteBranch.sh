printf "\n"
echo "Energy Time Series Modeling in C# using rDotNet"
printf "\n"
echo "."
echo ".."
echo "..."
echo "...."
echo "....."
echo "...... Getting remote new branch repository!"
echo "....."
echo "...."
echo "..."
echo ".."
echo "."

currentBranch=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')



git add .
timestamp=$(date +"%D %T")

git commit -m "Branch $currentBranch ($timestamp): Energy Time Series Modeling in C# using rDotNet"

echo "Updating current branch!"
git push

if [[ "$(git push --porcelain)" == *"Done"* ]]
then
  echo "Git Push was successful!"
fi

echo "\n\n\n List of loacl and remote branches:"
git branch -a

echo "\n\n Which branch would you like to pull? "
read thisBranch

git branch --set-upstream-to=origin/$thisBranch $thisBranch
git pull
printf "\n \n"

git branch -a

echo "Press any key to continue"
while [ true ] ; do
read -t 2 -n 1
if [ $? = 0 ] ; then
exit ;
else
echo -n "."
fi
done

printf "\n \n"