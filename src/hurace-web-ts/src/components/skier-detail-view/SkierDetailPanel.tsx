import React from 'react';
import { Skier } from '../../interfaces/Skier';
import styled from 'styled-components';

const DetailPanel = styled.div`
    display: flex;
    flex-direction: column;
`;

const Name = styled.span`
    font-size: 25px;
`;

export const SkierDetailPanel: React.FC<{ skier: Skier }> = ({ skier }) => {
    return (
        <DetailPanel>
            <Name>
                {skier.firstName} {skier.lastName}
            </Name>
            <span>{skier.gender.genderDescription}</span>
            <span>{skier.dateOfBirth.toDateString()}</span>
            <span>
                {skier.country.countryCode}-{skier.country.countryName}
            </span>
        </DetailPanel>
    );
};
