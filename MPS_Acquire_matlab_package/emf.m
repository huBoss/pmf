function [ e, grad ] = emf( cb, co, k, n )
% 根据电缆的参数cb计算8个线圈的电动势及其梯度
%   cb.p - 三元组，电缆的位置
%	cb.i - 三元组，电缆的电流向量，默认其模为I_max
%	co.p - 三元组，线圈的位置
%	co.n - 三元组，线圈的朝向
%	k - 常系数， N*f*mu_0*pi*a^2 N是线圈匝数，a是有效半径，f是频率一般为50Hz
%   n - n = length(co);
% return
%	|e| - 电动势的绝对值
%   grad(|e|) - 电动势绝对值的梯度
%   Author: Junyuan Hong
%   Date: 2014-10-06

% 	n = length(co);
    
	% ---- 计算径向向量r ----
	R = zeros(n,3);
	r = zeros(n,3);
	rr = zeros(n,1);
	for i=1:n
		R(i,:) = cb.p(:) - co(i).p(:); % R_0
	end
    ii = cb.i*cb.i';
	v = cb.i./sqrt(ii); % 电流的单位向量 v_cable
	r1 = R*v'; 
	for i=1:n
		r(i,:) = R(i,:) - r1(i)*v; % r
%         r(i,:) = r(i,:)./(r(i,:)*r(i,:)'); % vector(r)/r^2
		rr(i) = r(i,:)*r(i,:)';
    end
	% ---- 计算径向向量r ---- END ----

	% ---- 计算电动势e ----
    grad = zeros(n,6);
	e = zeros(1,n);
	for i = 1:n
        % ---- 计算电动势e ----
		e(i) = k*(cross(cb.i, R(i,:))*co(i).n')/rr(i); % (V)
        % ---- 计算电动势对cb.p的偏导dR ----
        grad(i,1:3) =   ( k*cross(co(i).n, cb.i)   -   2*e(i)*r(i,:));
        % ---- 计算电动势对cb.i的偏导dR ----
        grad(i,4:6) = - ( k*cross(co(i).n, R(i,:)) - 2*(R(i,:)*cb.i')*e(i)*r(i,:)/ii );
        grad(i,:) = sign(e(i)) * grad(i,:) / rr(i);
        e(i) = abs(e(i));
%         e(i) = e(i)*1000; % mV 如果使用mV作为单位的话能在MinFunc的使用中获得更高的精度
	end
	% ---- 计算电动势e ---- END ----
    
    
end

