function [  ] = OptimizeParams( filestr )
%优化参数
%   被优化的参数有：
%   - K 测量系数，实际的电动势e = dot(K,e);K有8个分量，分别对应8个线圈。
%   - k 模型系数，计算的电动势需要乘以这个参数

    %% init
    % 全局变量，需要在外部设置好
    % c 线圈参数
    % k 计算电动势用的系数
    % n 线圈个数
    % e0 测量得到的电动势
    global c k k0 n e0 e p i optimParam % 设置全局变量，方便使用MinFunc，因为有些参数不方便直接传递

%     load coils.mat % put the coils.mat file in same folder
    load nck.mat % load n c k
    optimParam = [];
    k0 = k;
%     c(7).n = [0,0,0]; % ATTENTION
    
    if isempty(filestr)
%         filestr = 'D:\Docs\code\gitlab\pmf\MPS_Acquire_matlab_package\data\NorthGateRoadGrassS2N4(1m)\30_14-11-04_11_28_50.mat';
%         filestr = 'D:\Docs\code\gitlab\pmf\MPS_Acquire_matlab_package\data\NorthGateRoadGrassS2N3(-1m)\eloop.mat';
        filestr = 'H:\temp\D\eloop_m1.mat';
    end
%     load(filestr,'e');
    trainset = 40:70;
    load(filestr, 'eloop');
    e(1:n,1:31) = eloop(:,trainset);
    filestr = 'H:\temp\D\eloop_1.mat';
    trainset = 60:90;
    load(filestr, 'eloop');
    e(1:n,32:62) = eloop(:,trainset);
    e0 = e;
    
    %% 
%     minFunc
    K = ones(8,1);
    ke = 1;
    p = [0,-1,-0.3];
    i = [100,0,0];
%     e0 = e'*K;%dot(e,K);
%     [err, grad] = func_emf([p,ke*i]')
%     v = OptimizeParamsFunc([K',ke]')
    m = [K',ke]';%K;%[K',ke]';
%     m = K;
    
%     options.progTol = 1e-12;
    options.Method = 'csd';
%     options.FunEvals = 1000; % 默认1000
%     options.MaxIter = 600;
%     options.Display = 'off';
    options.numDiff = 2;
    
    [m1,~,exitflag,output] = minFunc(@OptimizeParamsFunc, m, options);
    disp(m1(1:8)');
    disp(['ke ' num2str(m1(9))]);
%     disp(m1(9));
    
    K = m1(1:8);
%     ke = m1(9);
%     filestr = 'D:\Docs\code\gitlab\pmf\MPS_Acquire_matlab_package\data\NorthGateRoadGrassS2N4(1m)\eloop.mat';
%     filestr = 'D:\Docs\code\gitlab\pmf\MPS_Acquire_matlab_package\data\NorthGateRoadGrassS2N3(-1m)\eloop.mat';
    load(filestr, 'eloop');
    
    testset = 1:size(eloop,2);
    solveYZ = zeros(2,size(eloop,2));
    for ii=1:size(eloop,2)
        e = (K.*eloop(:,ii))';
        k = k*ke;

        %% searchCable进行求解
        disp('================');
        disp('* search cable...');
        [p, i] = searchCable(e',[0,-0,-0.2, 100, 0, 0],[]); % ATTENTION 这里忽略了7号线圈ignore coil7
        disp('* search cable...done');

        %% show result
        disp('===============');
        disp(['estimate result']);
        disp(['position: ' num2str(p)]);
        disp(['current:  ' num2str(i)]);
        
        solveYZ(:, ii) = p(2:3);
    end
    
    save('tempSolveYZ','solveYZ');
    
    plot(solveYZ','.');
%     hold on;
%     plot(solveYZ','.');
    legend('Y','Z');
    grid on;
    ylabel('distance/m');
    title('train result');
    axis([min(testset),max(testset),-1.2,1.2]);

end

