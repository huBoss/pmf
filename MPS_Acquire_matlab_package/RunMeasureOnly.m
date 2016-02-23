function [  ] = RunMeasureOnly( dataFolder, N )
%��������������������������

    % MeasureRate = 8960;
p = [];
i = [];
e0 = [];
e = [];
errmap = [];

plotData = zeros(N,8);

for ii=1:N
%% Acquire data
    sampleRate = 16000; % 64k sps
    bulkSize = 1024;
    bulks = 10;
    bufSize = bulks * bulkSize;
    TimeSize = bufSize / sampleRate;
    Time = linspace(-TimeSize/2, TimeSize/2, bufSize);

    channels = 8;
    [data] = mps_mex86(bulks, 1, sampleRate/1000);
    if isempty(data)
        return
    end
    data1 = data(1,:);
    save(['data/' dataFolder '/' datestr(now,'yy-mm-dd_HH_MM_SS_') num2str(ii,'%03d')  '.mat'], 'data1');
%     mn = repmat( mean(data,2),1,bufSize); % ȡ��ֵ
%     data = data-mn;
    % plot(1:(bufSize), data(1:channels,1:bufSize));
    er = zeros(1,8);

    % data = mps_mex86(bulks) - mn; % ����ȥ����һ�䣬��Ư��FFT�п��Ա�����Ƶ��Ϊ0��������ź�?��ʵ�ز��ԡ�
    for i = 1:channels
        er(i) = Get50Hz(Time, data(i,:)); % ��FFT��ȡ50Hz���źš�����û��50Hz���źš���ʵ�ز��ԡ�
    end
    disp(['measuered EMF(V): ' num2str(e)]);
    % check the signal amp is large enough to calculate
    if max(er) < 0.1
        disp('WARN: Measuered EMF is too low, can not use to estimate the cable. Continue anyway?');
    end
    disp(['MeasureRate: ' num2str(MeasureRate)]);
    % e = er/MeasureRate;
    % disp(['Real EMF(V): ' num2str(e)]);

%% plot
    plotData = [ er ; plotData(1:end-1,:)];
    plot(plotData);
    legend('1','2','3','4','5','6','7','8');
    drawnow;
    pause(0.1);

end



end

