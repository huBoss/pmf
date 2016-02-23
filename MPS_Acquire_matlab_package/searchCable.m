function [ cableP, cableI ] = searchCable( mE, startPos, unableCoils )
%��������ĵ綯��ʸ����1x8�������µĲ�����pλ�ã�i��������
% require files:
% emf.m
% func_emf.m
% �����ʾ�Ҳ���MinFunc�����MinFunc��ӵ�Matlab·�����档
% Author: Junyuan Hong
% Date: 2014-10-06
% Update: 2014-10-08 (from sovleTest.m)
% @input:
%  - mE: �����ĵĵ綯�ƣ���λ��V����Vpp
%  - startPos: ��ʼλ��m=[p_start,i_start]
%  - unableCoils: ʹ���ض���ŵ���Ȧ�������ã����ض���Ȧ����ֵ���Բ���ȷʱʹ�á�
% @output:
%  - cableP: ����λ��
%  - cableI: ���µ�������

% plotFlag: <0, not plot figure; 1. ֻ�����ݶ�ʸ����; 2. ֻ��������; 3. 1+�ݶ��½�����; 4.
% 2+�ݶ��½�����; 5. 2+��Ȧ���Ǻű��
% minfuncFlag: true ʹ���ݶ��½��㷨(MinFunc)
plotFlag = 0;
minfuncFlag = true;
% function [] = solveTest(plotFlag, minfuncFlag)

%% init data
% clear
global c k n e0 optimParam % ����ȫ�ֱ���������ʹ��MinFunc����Ϊ��Щ����������ֱ�Ӵ���
% optimParam ��������Щ�����������Ż�������optimCB�ṩ��ֵ��
optimParam = [];
% ����ϵ��cmΪ��λ��������Ҫ����100��n�ǵ�λ����������
% % +x
% c(1).p = [45, 0, 5]/100;
% c(1).n = [1, 0, 0];
% c(2).p = [45, 0, 39]/100;
% c(2).n = [1, 0, 0];
% 
% % +y side
% c(3).p = [4, 15, 3]/100;
% c(3).n = [0, 1, 0];
% c(4).p = [4, 15, 43]/100;
% c(4).n = [0, 1, 0];
% 
% % -y side
% c(5).p = [40, -15, 3]/100;
% c(5).n = [0, 1, 0];
% c(6).p = [40, -15, 43]/100;
% c(6).n = [0, 1, 0];
% 
% % +z side
% c(7).p = [32,0,0]/100;
% c(7).n = [0,0,1];
% c(8).p = [8,0,0]/100;
% c(8).n = [0,0,1];

load coils.mat % put the coils.mat file in same folder

for index = unableCoils
    c(index).n = [0,0,0];
end
% c(7).n = [0 0 0];
% c(2).n = [0 0 0];
% c(4).n = [0 0 0];

% cable
if isempty(mE)
    disp('WARN: no input EMF, use simulate value');
    cb.p = [0,-1,-.4];
    cb.i = 400 * [1,0,0];
end

% k ������������������Ȧ��һЩ��������
N = 2000;
f = 50;
mu_0 = 4*pi*1e-7;
a = 0.03;
k = N*f*mu_0*pi*a^2;

%% simulate measured EMF
n = length(c);
if isempty(mE)
    [e0] = emf(cb,c,k, n); % ������ʵ��ʹ����ʹ�ò���ֵ����;
    disp(['real e0(V):' num2str(e0)]);
    e0 = e0 + randn(1,8)/1000; % mV noise
    disp(['measuered e0:' num2str(e0)]);
else
    e0 = mE;
    disp(['real e0(V):' num2str(e0')]);
end

%% solve cable
% ʹ���ݶ��½��㷨
% Ѱ������ʼλ��
% m = [cb0.p cb0.i];
if isempty(startPos)
    m = [0,-0,-0.2, 200, 0, 0];
else
    m = startPos;
end
% m = [cb.p, cb.i*2];

% m(optimParam) = optimCB(optimParam);

%% plot
if plotFlag>0
    figure
    width = 50;
    x = zeros(width^2,1);
    y = zeros(width^2,1);
    u = zeros(width^2,1);
    v = zeros(width^2,1);
    Z = zeros(width^2,1);
    i = 0;

    % ��ͼ�ķ�Χ����
    % ixx = [linspace(-0.4, 0.4,width)];
    % iyy = [linspace(-0.3, 0.7,width)];

    ixx = [linspace(-2.5, 1.5,width)];
    iyy = [linspace(-1.5, -0.4,width)];

    % ixx = [linspace(-0.6, -0.4,width)];
    % iyy = [linspace(-0.6, -0.4,width)];
    % ����������ݶ�ʸ��
    for ix = ixx
        for iy = iyy
            i = i+1;
            [h, g] = func_emf_raw([0 ix iy m(4:6)]);
            x(i) = ix;
            y(i) = iy;
            u(i) = - g(2);
            v(i) = - g(3);
            Z(i) = h;
        end
    end
    Z = reshape(Z,width,width);
    if plotFlag==1
        quiver(x,y,u,v,8); % �����ݶ�ʸ����
    end
    hold on
    [X,Y] = meshgrid(ixx,iyy);
    if plotFlag==2
        mesh(X,Y,Z); % ��������
    end
    % �򵥵��ݶ��½�����
    % Line1
    len = 200; % ���ò���Ϊ200
    X1(1) = 0.4;
    Y1(1) = -0.5;
    lambda = 0.1; % ����
    height = 0.001;
    Z1(1) = 1;
    for i=1:len
        [h, g] = func_emf_raw([0 X1(i) Y1(i) m(4:6)]);
        if i>1 && (Z1(i-1) - h)<height % ����ÿ���½��Ĵ�С��������
    %         height = abs(h - Z2(i-1))
            lambda = lambda / 2;
        end
        X1(i+1) = X1(i) - lambda*g(2)/norm(g);
        Y1(i+1) = Y1(i) - lambda*g(3)/norm(g);
        Z1(i) = h;
    end
    % Line2
    X2(1) = -0;
    Y2(1) = -0.2;
    Z2(1) = 1;
    lambda = 0.1;
    height = 0.001;
    %     [h, g] = func_emf([0 X2(1) Y2(1) 200 0 0]);
    %     norm(g)
    for i=1:len
        [h, g] = func_emf_raw([0 X2(i) Y2(i) m(4:6)]);
        if i>1 && (Z2(i-1) - h)<height
    %         height = abs(h - Z2(i-1))
            lambda = lambda / 2;
        end
        X2(i+1) = X2(i) - lambda*g(2)/norm(g);
        Y2(i+1) = Y2(i) - lambda*g(3)/norm(g);
        Z2(i) = h;
    end
    % Line3
    X3(1) = 0.5;
    Y3(1) = -0.4;
    Z3(1) = 1;
    lambda = 0.1;
    height = 0.001;
    %     [h, g] = func_emf([0 X2(1) Y2(1) 200 0 0]);
    %     norm(g)
    for i=1:len
        [h, g] = func_emf_raw([0 X3(i) Y3(i) m(4:6)]);
        if i>1 && (Z3(i-1) - h)<height
    %         height = abs(h - Z2(i-1))
            lambda = lambda / 2;
        end
        X3(i+1) = X3(i) - lambda*g(2)/norm(g);
        Y3(i+1) = Y3(i) - lambda*g(3)/norm(g);
        Z3(i) = h;
    end
    cb1.p = [0 X2(end) Y2(end)];
    cb1.i = m(4:6);
    plot3(X1(1:len),Y1(1:len),Z1,'m.-',X2(1:len),Y2(1:len),Z2,'b.-',X3(1:len),Y3(1:len),Z3,'g.-',cb.p(2),cb.p(3),0,'p-');
    % ������Ȧλ��
%     for i=1:8
%         H = plot3(c(i).p(2),c(i).p(3),0.1, 'rp');
%         set(H,'MarkerSize', 12, 'MarkerFaceColor', 'r');
%     end
    xlabel('y/m');
    ylabel('z/m');
    zlabel('\Delta/V');
    % axis([-1 1 -1 1 0 1e-8]);
    grid on
    hold off
    
    % plot the \Delta-I figure
    figure
    [val grad] = func_emf_raw(m);
    disp([num2str(val) num2str(grad)]);
    width = 200;
    I = linspace(m(4)-400,m(4)+100,width);
    D = zeros(1,width);
    G = zeros(1,width);
    for i = 1:width
        [D(i) g] = func_emf_raw([m(1:3),I(i),0,0]);
        G(i) = g(4);
    end
    plot(I,D,I,G);
end

%% use myMinFunc to calculate the minimum
% [cb1m] = myMinFunc(@func_emf_raw,m); % �Զ����myMinFunc

%% use fminunc to calculate the minimum
% % options = optimoptions('fminunc','MaxFunEvals',6000,'MaxIter',600,'GradObj','on');
% % options = optimoptions('fminunc','MaxFunEvals',6000,'MaxIter',600,'Algorithm','quasi-newton');
% % [cb1m,fval,exitflag,output] = fminunc(@func_emf_raw,m,options);

%% use MinFunc
if minfuncFlag == true
%     options.DerivativeCheck = 'on'; % �Ƚ���ֵ�ݶȺ��û��ṩ���ݶ�
%     % options.NumDiff = 2; % ʹ����ֵ�ݶȣ���ʱDerivativeCheck��Ч
%     options.progTol = 1e-15;
%     options.optTol = 1e-7;
%     options.Method = 'newton0';
%     options.FunEvals = 1000; % Ĭ��1000
%     options.MaxIter = 600;
%     options.Display = 'on';
    
    disp('* search current...');
    optimCB = m';
    [cb1m,fval, currentIter, exitflag, output] = searchCurrent(m); % ���Ż�
    cb1m = cb1m';

    disp(['searchCurrent Iter: ' num2str(currentIter)]);
    if exitflag == 0
        disp('flag: 0 ��������������Χ');
    end
    disp(['fval: ' num2str(fval)]);
    disp(output.message);

    cb1.p = cb1m(1:3);
    cb1.i = cb1m(4:6);
    
end

%% show the result
disp('=====================')
if isempty(mE)
    disp(['init of cable:      ' num2str(cb.p) '  ' num2str(cb.i)]);
end
disp(['start position:     ' num2str(m)]);
disp(['the solve of cable: ' num2str(cb1.p) '  ' num2str(cb1.i)]);
if isempty(mE)
    disp(['Diff:               ' num2str(cb1.p - cb.p) '  ' num2str(cb1.i - cb.i)]);
end
cableP = cb1.p;
cableI = cb1.i;



end