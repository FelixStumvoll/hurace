import React, { useState, useEffect } from 'react';
import { Skier } from '../../interfaces/Skier';
import styled from 'styled-components';
import { Discipline } from '../../interfaces/Discipline';
import { setStateAsync } from '../../common/stateSetter';
import { getDisciplinesForSkier } from '../../api';

const DetailPanel = styled.div`
    padding: 10px;
    display: flex;
    flex-direction: column;
`;

const StatsGrid = styled.div`
    display: grid;
    grid-template-columns: auto auto;
    column-gap: 5px;
    grid-template-rows: repeat(3, auto);
`;

const Name = styled.span`
    font-size: 25px;
    font-weight: bold;
`;

const StatLabel = styled.label`
    font-weight: bold;
`;

const DisciplineText = styled.span`
    word-break: break-all;
`;

export const SkierDetailPanel: React.FC<{ skier: Skier }> = ({ skier }) => {
    const [disciplines, setDisciplines] = useState<Discipline[] | undefined>(
        undefined
    );

    useEffect(() => {
        setStateAsync(setDisciplines, getDisciplinesForSkier(skier.id));
    }, [disciplines, skier]);

    return (
        <DetailPanel>
            <Name>
                {skier.firstName} {skier.lastName}
            </Name>
            <StatsGrid>
                <StatLabel>Geschlecht:</StatLabel>
                <span>{skier.gender.genderDescription}</span>
                <StatLabel>Geburtsdatum:</StatLabel>
                <span>{skier.dateOfBirth.toDateString()}</span>
                <StatLabel>Land:</StatLabel>
                <span>{skier.country.countryName}</span>
                <StatLabel>Disziplinen:</StatLabel>
            </StatsGrid>
            <DisciplineText>
                {disciplines &&
                    disciplines.map(d => d.disciplineName).toString()}
            </DisciplineText>
        </DetailPanel>
    );
};
