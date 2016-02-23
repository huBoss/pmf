function [ errmap, yrange, zrange, u, v ] = CreateErrorMap( i, ErrorMapRect )
%创建ErrorMapRect范围内的errormap，这时应该用全局变量
% @input
%   - i: current
%   - ErrorMapRect: [ymin ymax zmin zmax ynum znum]

errmap = zeros(ErrorMapRect(5), ErrorMapRect(6));
u = zeros(ErrorMapRect(5), 1);
v = zeros(ErrorMapRect(6), 1);

yrange = linspace(ErrorMapRect(1),ErrorMapRect(2),ErrorMapRect(5));
zrange = linspace(ErrorMapRect(3),ErrorMapRect(4),ErrorMapRect(6));

for iy = 1:ErrorMapRect(5)
    for iz = 1:ErrorMapRect(6)
        [errmap(iy,iz) g] = func_emf([0,yrange(iy),zrange(iz),i]');
        u(iy) = - g(2);
        v(iz) = - g(3);
    end
end


end

