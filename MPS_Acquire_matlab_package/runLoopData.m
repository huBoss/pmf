function [  ] = runLoopData( tag, t )
%循环 获取数据 t次
%   - tag 将用于保存数据的文件夹名字
%   - t, times 循环次数

if isempty(t) || t==0
    t=500;
end
if isempty(tag)
    tag = datestr(now,'yy-mm-dd_HH_MM_SS');
end
%% Acquire data
sampleRate = 8000; % 64k sps
bulkSize = 1024;
bulks = 10;
bufSize = bulks * bulkSize;
TimeSize = bufSize / sampleRate;
Time = linspace(-TimeSize/2, TimeSize/2, bufSize);

channels = 8;
verb = 1;
% mn = repmat( mean(data,2),1,bufSize); % 取均值
% data = data-mn;

% plot(1:(bufSize), data(1:channels,1:bufSize));
er = zeros(1,8);

%% Loop
eloop = zeros(8,t);
mkdir(['data/' tag '/']);
for ii=1:t
    disp(['--------------' num2str(ii) '-----------------']);
%     [p, i, e0, e, errmap, data] = runAcquireAndSearchFunc([],[],false); 
    [ data ] = mps_mex86(bulks, verb, sampleRate/1000);
    plot(e,'.-');
    title('e');
    save(['data/' tag '/' num2str(ii,'%02d') datestr(now,'_yy-mm-dd_HH_MM_SS')],'p', 'i', 'e0', 'e','errmap', 'data');
    eloop(:,ii) = e';
%     pause(0.1);
    drawnow;
end
save(['data/' tag '/eloop'], 'eloop');
plot(eloop');
legend('1','2','3','4','5','6','7','8');
title('eloop');


end

