function [p, i, e1, e, errmap, data] = runAcquireAndSearchFunc(MeasureRate, ErrorMapRect, showErrorMap)
% ��MPS�ɼ����ݣ�����searchCable������⡣�������Ľ������Ȧ��������coils.mat�С�Ŀǰֻ�ܶԳ�����ǰ���ĵ��½��м���
% require files:
%  searchCable.m
%  Get50Hz.m
%  MPS-140801.dll
%  mps_mex86.mexw32
%  coils.mat
% @intput
%  - MeasureRate: �Ŵ������˲���������ϵ��������Ӳ�������� default:8960
%  - ErrorMapRect: [ymin ymax zmin zmax ynum znum]��default: [-1,1,-2,-0.1,20,20]
%  - showErrorMap: true, ����Errormap
% @return
%  - p: ���µ�λ��
%  - i: ���µĵ���ʸ���������������I*(1,0,0);
%  - e0: ������µķ�������ȷ�Ļ���Ӧ��Ϊ0����������Сֵ
%  - e: 8����Ȧ�ĵ綯��
%  - errmap����ErrorMapRect������errmap

%% process params
if isempty(MeasureRate)
    MeasureRate = 8960;
end
% if isempty(ErrorMapRect)
%     ErrorMapRect = [-1,1,-2,-0.1,20,20];
% end
p = [];
i = [];
e1 = [];
e = [];
errmap = [];

%% Acquire data
sampleRate = 8000; % 64k sps
bulkSize = 1024;
bulks = 10;
bufSize = bulks * bulkSize;
TimeSize = bufSize / sampleRate;
Time = linspace(-TimeSize/2, TimeSize/2, bufSize);

channels = 8;
verb = 1;
[data] = mps_mex86(bulks, verb, sampleRate/1000);
% mn = repmat( mean(data,2),1,bufSize); % ȡ��ֵ
% data = data-mn;

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
e = er/MeasureRate;
disp(['Real EMF(V): ' num2str(e)]);
% check the direction
e0 = e(1);
if e0>0.0001
    disp([' ***WARN: e1 ' num2str(e0) ' is too large, you may not in the right direction']);
end
s = sort(e);
if e(2) ~= e0
    disp(' ** WARN: e(1) is not the minium one, not in the right direction');
%         return; 
end

% disp('press any key to continue or ctrl-C to quit...');
% pause

%% searchCable�������
disp('================');
disp('* search cable...');
[p, i] = searchCable(e,[0,-0,-0.2, 10, 0, 0],[7]);
disp('* search cable...done');

%% create errormap
fprintf('* create errormap...');
if isempty(ErrorMapRect)
    ErrorMapRect = [-p(2)-20 p(2)+20 p(3)-10 p(3)+10 10 10];
end
[errmap, X, Y] = CreateErrorMap(i, ErrorMapRect);
if showErrorMap
    mesh(Y,X,errmap);
    xlabel('z/m');ylabel('y/m');
    title(['Error Map Around Solution(i: ' num2str(i)]);
    view(90,0);
end
fprintf('done\n');

%% show result
disp('===============');
disp(['estimate result']);
disp(['position: ' num2str(p)]);
disp(['current:  ' num2str(i)]);

%% save var
save(['data/' datestr(now,'yy-mm-dd_HH_MM_SS')]);

end

