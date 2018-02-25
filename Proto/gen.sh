for f in $(ls ./proto/*.proto); do
echo $f
./protoc -I=./proto --csharp_out=./output/ $f
done

for f in $(ls ./output/*); do 
filename=$(basename "$f")
filename="${filename%.*}"
cp $f ../Common/${filename}.pb.cs
done
 