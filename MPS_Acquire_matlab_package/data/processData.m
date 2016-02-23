function [ data ] = processData( dirName )
%UNTITLED 此处显示有关此函数的摘要
%   此处显示详细说明

verb = false;
verb = true;
if ~exist(dirName)
    disp('no such dir');
    return;
end
filesStruct = dir(dirName);
n = length(filesStruct);
disp([ dirName ' include ' num2str(n-2) ' files']);

data = zeros(8,n-2);

for ii=3:(n-1)
    if verb
        disp(['load ' num2str(ii-2) ' file:' filesStruct(ii).name]);
        disp(['         ' filesStruct(ii).date]);
    end
    load(fullfile(dirName, filesStruct(ii).name));
    data(:,ii-2) = e';
end
disp(['load ' num2str(ii-2)  ' files success']);

figure;
plot(data(1:7,:)');
legend('1','2','3','4','5','6','7');
xlabel('points');
ylabel('signal/Vpp');
title(dirName);


end

