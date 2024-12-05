using System.Collections.Generic;
using System.Linq;
using BaiTap.Models;

namespace BaiTap.Services
{
    public class Sosanh
    {
        public string SoSanhSP1(ChiTietSanPham product1, ChiTietSanPham product2)
        {
            List<string> comparisons = new List<string>();

            if (product1.ManHinh != product2.ManHinh)
                comparisons.Add($"Màn h?nh: {product1.ManHinh} vs {product2.ManHinh}");
            if (product1.HeDieuHanh != product2.HeDieuHanh)
                comparisons.Add($"H? ði?u hành: {product1.HeDieuHanh} vs {product2.HeDieuHanh}");
            if (product1.CameraTruoc != product2.CameraTruoc)
                comparisons.Add($"Camera trý?c: {product1.CameraTruoc} vs {product2.CameraTruoc}");
            if (product1.CameraSau != product2.CameraSau)
                comparisons.Add($"Camera sau: {product1.CameraSau} vs {product2.CameraSau}");
            if (product1.Chip != product2.Chip)
                comparisons.Add($"Chip: {product1.Chip} vs {product2.Chip}");
            if (product1.RAM != product2.RAM)
                comparisons.Add($"RAM: {product1.RAM} vs {product2.RAM}");
            if (product1.BoNhoTrong != product2.BoNhoTrong)
                comparisons.Add($"B? nh? trong: {product1.BoNhoTrong} vs {product2.BoNhoTrong}");
            if (product1.Sim != product2.Sim)
                comparisons.Add($"Sim: {product1.Sim} vs {product2.Sim}");
            if (product1.Pin != product2.Pin)
                comparisons.Add($"Pin: {product1.Pin} vs {product2.Pin}");

            return string.Join("\n", comparisons);
        }

        public string SoSanhSP2(ChiTietSanPham product)
        {
            List<string> evaluations = new List<string>();

            if (!string.IsNullOrEmpty(product.ManHinh))
                evaluations.Add($"Màn h?nh: {product.ManHinh}");
            if (!string.IsNullOrEmpty(product.HeDieuHanh))
                evaluations.Add($"H? ði?u hành: {product.HeDieuHanh}");
            if (!string.IsNullOrEmpty(product.CameraTruoc))
                evaluations.Add($"Camera trý?c: {product.CameraTruoc}");
            if (!string.IsNullOrEmpty(product.CameraSau))
                evaluations.Add($"Camera sau: {product.CameraSau}");
            if (!string.IsNullOrEmpty(product.Chip))
                evaluations.Add($"Chip: {product.Chip}");
            if (!string.IsNullOrEmpty(product.RAM))
                evaluations.Add($"RAM: {product.RAM}");
            if (!string.IsNullOrEmpty(product.BoNhoTrong))
                evaluations.Add($"B? nh? trong: {product.BoNhoTrong}");
            if (!string.IsNullOrEmpty(product.Sim))
                evaluations.Add($"Sim: {product.Sim}");
            if (!string.IsNullOrEmpty(product.Pin))
                evaluations.Add($"Pin: {product.Pin}");

            return string.Join("\n", evaluations);
        }
    }
}
