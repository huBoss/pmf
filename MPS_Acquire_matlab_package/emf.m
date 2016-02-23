function [ e, grad ] = emf( cb, co, k, n )
% ���ݵ��µĲ���cb����8����Ȧ�ĵ綯�Ƽ����ݶ�
%   cb.p - ��Ԫ�飬���µ�λ��
%	cb.i - ��Ԫ�飬���µĵ���������Ĭ����ģΪI_max
%	co.p - ��Ԫ�飬��Ȧ��λ��
%	co.n - ��Ԫ�飬��Ȧ�ĳ���
%	k - ��ϵ���� N*f*mu_0*pi*a^2 N����Ȧ������a����Ч�뾶��f��Ƶ��һ��Ϊ50Hz
%   n - n = length(co);
% return
%	|e| - �綯�Ƶľ���ֵ
%   grad(|e|) - �綯�ƾ���ֵ���ݶ�
%   Author: Junyuan Hong
%   Date: 2014-10-06

% 	n = length(co);
    
	% ---- ���㾶������r ----
	R = zeros(n,3);
	r = zeros(n,3);
	rr = zeros(n,1);
	for i=1:n
		R(i,:) = cb.p(:) - co(i).p(:); % R_0
	end
    ii = cb.i*cb.i';
	v = cb.i./sqrt(ii); % �����ĵ�λ���� v_cable
	r1 = R*v'; 
	for i=1:n
		r(i,:) = R(i,:) - r1(i)*v; % r
%         r(i,:) = r(i,:)./(r(i,:)*r(i,:)'); % vector(r)/r^2
		rr(i) = r(i,:)*r(i,:)';
    end
	% ---- ���㾶������r ---- END ----

	% ---- ����綯��e ----
    grad = zeros(n,6);
	e = zeros(1,n);
	for i = 1:n
        % ---- ����綯��e ----
		e(i) = k*(cross(cb.i, R(i,:))*co(i).n')/rr(i); % (V)
        % ---- ����綯�ƶ�cb.p��ƫ��dR ----
        grad(i,1:3) =   ( k*cross(co(i).n, cb.i)   -   2*e(i)*r(i,:));
        % ---- ����綯�ƶ�cb.i��ƫ��dR ----
        grad(i,4:6) = - ( k*cross(co(i).n, R(i,:)) - 2*(R(i,:)*cb.i')*e(i)*r(i,:)/ii );
        grad(i,:) = sign(e(i)) * grad(i,:) / rr(i);
        e(i) = abs(e(i));
%         e(i) = e(i)*1000; % mV ���ʹ��mV��Ϊ��λ�Ļ�����MinFunc��ʹ���л�ø��ߵľ���
	end
	% ---- ����綯��e ---- END ----
    
    
end

